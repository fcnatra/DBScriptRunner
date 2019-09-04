using DbScriptRunner.Services;
using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using DbScriptRunnerLogic.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbScriptRunner.UI
{
    public partial class frmMain : Form
    {
        private AppData<Database> _databasesAppData;
        private AppData<Script> _scriptsAppData;

        public frmMain()
        {
            InitializeComponent();

            ConfigureLogicDependencies();

            RecoverLastStatus();

            InitializeControlsDesign();
        }

        private void RecoverLastStatus()
        {
            _databasesAppData.RecoverLastStatus();
            _scriptsAppData.RecoverLastStatus();
        }

        private void ConfigureLogicDependencies()
        {
            _databasesAppData = new AppData<Database>();
            _databasesAppData.Persistence.Repository = new TextFileRepository();

            _scriptsAppData = new AppData<Script>();
            _scriptsAppData.Persistence.Repository = new TextFileRepository();
        }

        private void InitializeControlsDesign()
        {
            InitializeUIForDatabasesListView();
            PopulateListView(_databasesAppData.Instances, lvDatabases, null);
            UpdateConfigFileListHeader(_databasesAppData);

            InitializeUIForScriptsListView();
            PopulateListView(_scriptsAppData.Instances, lvScripts, null);
            UpdateConfigFileListHeader(_scriptsAppData);

            HighlightListViewHeader(lvDatabases);

            UpdateListOfLastOpenedFiles();
        }

        private void RemoveItemsBelowSeparator(ToolStripMenuItem menuOption, ToolStripSeparator separator)
        {
            var indexOfSeparator = menuOption.DropDownItems.IndexOf(separator);
            var maxIndex = menuOption.DropDownItems.Count - 1;
            for (int i = indexOfSeparator; i < maxIndex; i++)
                menuOption.DropDownItems.RemoveAt(indexOfSeparator+1);
        }

        private void InitializeUIForScriptsListView()
        {
            lvScripts.Columns.Add(new ColumnHeader { DisplayIndex = 0, Width = 70, Text = "Order", Name = "lvScriptsHeaderOrder" });
            lvScripts.Columns.Add(new ColumnHeader { DisplayIndex = 1, Width = 500, Text = "Name", Name = "lvScriptsHeaderName" });
            lvScripts.Columns[0].TextAlign = HorizontalAlignment.Center;
            lvScripts.View = View.Details;
        }

        private void InitializeUIForDatabasesListView()
        {
            lvDatabases.Columns.Add(new ColumnHeader { DisplayIndex = 0, Width = 70, Text = "Order", Name = "lvDatabasesHeaderOrder" });
            lvDatabases.Columns.Add(new ColumnHeader { DisplayIndex = 1, Width = 300, Text = "Name", Name = "lvDatabasesHeaderName" });
            lvDatabases.Columns[0].TextAlign = HorizontalAlignment.Center;
            lvDatabases.View = View.Details;
        }

        private void LastOpenedFiles_Click<T>(ToolStripMenuItem menuItem, AppData<T> appData) where T : INamed, new()
        {
            var listView = (appData is AppData<Script>) ? lvScripts : lvDatabases;
            var fileOpened = OpenConfigurationFile(appData, listView, menuItem.Text);
            if (!fileOpened)
                RemoveFileFromLastOpenedFiles(menuItem.Text, appData);
        }

        private void RemoveFileFromLastOpenedFiles<T>(string fullFileName, AppData<T> appData) where T : INamed, new()
        {
            if (DialogResult.No == CommonDialogs.AreYouSure($"Remove file from the list?\n{fullFileName}"))
                return;

            appData.RemoveFileNameFromLastOpenedFiles(fullFileName);
            UpdateListOfLastOpenedFiles();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool continueClosing  = CheckConfigurationIsSaved(_databasesAppData, "Database");
            e.Cancel = !continueClosing;
            if (e.Cancel) return;

            continueClosing = CheckConfigurationIsSaved(_scriptsAppData, "Script");
            e.Cancel = !continueClosing;
        }

        private void toolbarMoveUp_Click(object sender, EventArgs e)
        {
            MoveItemsUp();
        }

        private void toolbarMoveDown_Click(object sender, EventArgs e)
        {
            MoveItemsDown();
        }

        private void toolbarRemove_Click(object sender, EventArgs e)
        {
            RemoveItemsOnFocusedList();
        }

        private void toolbarAddNewItem_Click(object sender, EventArgs e)
        {
            AddNewItem();
        }

        private void toolbarEdit_Click(object sender, EventArgs e)
        {
            EditSelectedItem();
        }

        private void menuOpenServersConfiguration_Click(object sender, EventArgs e)
        {
            SelectConfigurationFileToOpen(_databasesAppData, lvDatabases);
        }

        private void menuSaveServersConfiguration_Click(object sender, EventArgs e)
        {
            SaveConfigurationFile(_databasesAppData);
        }

        private void addServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewDatabaseItem(lvDatabases);
        }

        private void editSelectedServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedDatabaseItem(lvDatabases);
        }

        private void menuOpenScriptConfiguration_Click(object sender, EventArgs e)
        {
            SelectConfigurationFileToOpen(_scriptsAppData, lvScripts);
        }

        private void menuSaveScriptConfiguration_Click(object sender, EventArgs e)
        {
            SaveConfigurationFile(_scriptsAppData);
        }

        private void menuLoadScript_Click(object sender, EventArgs e)
        {
            AddNewItem();
        }
 
        private void menuRunScriptsSequentially_Click(object sender, EventArgs e)
        {
            var runner = new Runner();
            runner.Databases = (IEnumerable<Database>)_databasesAppData.Instances;
            runner.Scripts = (IEnumerable<Script>)_scriptsAppData.Instances;
            runner.DatabaseServiceFactory = new DatabaseServiceForSql();
            runner.Run();
        }

        private void listViewConfiguration_Enter(object sender, EventArgs e)
        {
            var focusedListView = (ListView)sender;
            var listViewToUnfocus = (focusedListView.Name == lvDatabases.Name) ? lvScripts : lvDatabases;

            HighlightListViewHeader((ListView)sender);
            UnHighlightListViewHeader(listViewToUnfocus);
        }

        private void listViews_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveItemsOnFocusedList();
                e.Handled = true;
            }
        }

        private void menuCreateNewServersConfiguration_Click(object sender, EventArgs e)
        {
            _databasesAppData.CreateNew();
            PopulateListView(_databasesAppData.Instances, lvDatabases);
        }

        private void menuCreateNewScriptConfiguration_Click(object sender, EventArgs e)
        {
            _scriptsAppData.CreateNew();
            PopulateListView(_scriptsAppData.Instances, lvScripts);
        }

        private void menuSaveServersConfigurationAs_Click(object sender, EventArgs e)
        {
            SaveConfigurationFileAs(_databasesAppData);
        }

        private void menuSaveScriptConfigurationAs_Click(object sender, EventArgs e)
        {
            SaveConfigurationFileAs(_scriptsAppData);
        }

        private void SaveConfigurationFileAs<T>(AppData<T> appData) where T : INamed, new()
        {
            var newAppData = appData.CreateNewFromThis();
            bool saved = SaveConfigurationFile(newAppData);
            if (saved)
            {
                appData.UpdateFrom(newAppData);
                UpdateListOfLastOpenedFiles();
            }
        }

        private void UnHighlightListViewHeader(ListView unfocusedListView)
        {
            var lblHeader = (unfocusedListView.Name == lvDatabases.Name) ? lblDatabasesTitle : lblScriptsTitle;
            lblHeader.BackColor = SystemColors.ControlLight;
            lblHeader.ForeColor = SystemColors.WindowText;
            lblHeader.Font = new Font(lblHeader.Font, FontStyle.Regular);
        }

        private void HighlightListViewHeader(ListView focusedListView)
        {
            var lblHeader = (focusedListView.Name == lvDatabases.Name) ? lblDatabasesTitle : lblScriptsTitle;
            lblHeader.BackColor = SystemColors.Highlight;
            lblHeader.ForeColor = SystemColors.HighlightText;
            lblHeader.Font = new Font(lblHeader.Font, FontStyle.Bold);
        }

        private ListView GetFocusedListBetweenDatabasesOrScripts()
        {
            if (lvDatabases.Focused || lblDatabasesTitle.BackColor == SystemColors.Highlight) return lvDatabases;
            else return lvScripts;
        }

        private void PopulateListView(IArrangeableList<INamed> datasource, ListView listView, List<int> indicesToSelect = null)
        {
            listView.Items.Clear();

            if (datasource == null || !datasource.Any()) return;

            listView.SuspendLayout();
            listView.Tag = datasource;
            int index = 0;
            foreach (var item in datasource)
            {
                ListViewItem lvItem = listView.Items.Add(index.ToString());
                lvItem.Tag = item;
                lvItem.SubItems.Add(item.Name);
                ++index;
            }

            SelectIndicesOnListView(listView, indicesToSelect);

            listView.ResumeLayout();
        }

        private void SelectIndicesOnListView(ListView lvControl, List<int> indicesToSelect)
        {
            if (indicesToSelect == null) return;

            var maxIndex = lvControl.Items.Count;
            foreach (var index in indicesToSelect)
                lvControl.SelectedIndices.Add(index);
        }

        private void RemoveItemsOnFocusedList()
        {
            var focusedListView = GetFocusedListBetweenDatabasesOrScripts();
            if (focusedListView == null) return;

            if (!IsThereAnyItemSelected(focusedListView))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }

            var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
            var datasource = (ArrangeableList<INamed>)focusedListView.Tag;
            var indicesToSelect = datasource.RemoveAt(selectedIndices);
            PopulateListView(datasource, focusedListView, indicesToSelect);
        }

        private void MoveItemsDown()
        {
            ListView focusedListView = GetFocusedListBetweenDatabasesOrScripts();
            if (focusedListView == null) return;

            if (!IsThereAnyItemSelected(focusedListView))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }

            var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
            var datasource = (ArrangeableList<INamed>)focusedListView.Tag;
            var indicesToSelect = datasource.MoveItemsDownOnePosition(selectedIndices);
            PopulateListView(datasource, focusedListView, indicesToSelect);
        }

        private void MoveItemsUp()
        {
            ListView focusedListView = GetFocusedListBetweenDatabasesOrScripts();
            if (focusedListView == null) return;

            if (!IsThereAnyItemSelected(focusedListView))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }

            var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
            var datasource = (ArrangeableList<INamed>)focusedListView.Tag;
            var indicesToSelect = datasource.MoveItemsUpOnePosition(selectedIndices);
            PopulateListView(datasource, focusedListView, indicesToSelect);
        }

        private void EditSelectedItem()
        {
            ListView focusedListView = GetFocusedListBetweenDatabasesOrScripts();
            if (focusedListView == null) return;

            if (focusedListView.SelectedIndices.Count != 1)
            {
                CommonDialogs.TellUserToSelectJustOneItem();
                return;
            }

            if (focusedListView == lvDatabases) EditSelectedDatabaseItem(focusedListView);
            else EditSelectedScriptItem(focusedListView);
        }

        private void EditSelectedScriptItem(ListView focusedListView)
        {
            var datasource = (ArrangeableList<INamed>)focusedListView.Tag;

            var editForm = new frmAddEditScript();
            editForm.MethodToGetErrorsOnItemInformation = _scriptsAppData.CheckForErrorsOnName;
            editForm.ItemInformation = (Script)focusedListView.SelectedItems[0].Tag;

            var dialogResult = editForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
                datasource.ReplaceAt(focusedListView.SelectedIndices[0], editForm.ItemInformation);
                PopulateListView(datasource, focusedListView, selectedIndices);
            }
        }

        private void EditSelectedDatabaseItem(ListView focusedListView)
        {
            var datasource = (ArrangeableList<INamed>)focusedListView.Tag;

            var editForm = new frmAddEditDatabase();
            editForm.MethodToGetErrorsOnItemInformation = _databasesAppData.CheckForErrorsOnName;
            editForm.DatabaseInformation = (Database)focusedListView.SelectedItems[0].Tag;

            var dialogResult = editForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
                datasource.ReplaceAt(focusedListView.SelectedIndices[0], editForm.DatabaseInformation);
                PopulateListView(datasource, focusedListView, selectedIndices);
            }
        }

        private void AddNewItem()
        {
            ListView focusedListView = GetFocusedListBetweenDatabasesOrScripts();
            if (focusedListView == null) return;

            if (focusedListView == lvDatabases) AddNewDatabaseItem(focusedListView);
            else AddNewScriptItem(focusedListView);
        }

        private void AddNewDatabaseItem(ListView focusedListView)
        {
            var addForm = new frmAddEditDatabase();
            addForm.MethodToGetErrorsOnItemInformation = _databasesAppData.CheckForErrorsOnName;
            var dialogResult = addForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                var databaseList = ((ArrangeableList<INamed>)_databasesAppData.Instances);
                if (databaseList == null)
                {
                    databaseList = new ArrangeableList<INamed>();
                    _databasesAppData.Instances = databaseList;
                }
                databaseList.Add(addForm.DatabaseInformation);

                var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
                PopulateListView(databaseList, focusedListView, selectedIndices);
            }
        }

        private void AddNewScriptItem(ListView focusedListView)
        {
            var addForm = new frmAddEditScript();
            addForm.MethodToGetErrorsOnItemInformation = _scriptsAppData.CheckForErrorsOnName;
            var dialogResult = addForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                var scriptList = ((ArrangeableList<INamed>)_scriptsAppData.Instances);
                if (scriptList == null)
                {
                    scriptList = new ArrangeableList<INamed>();
                    _scriptsAppData.Instances = scriptList;
                }
                scriptList.Add(addForm.ItemInformation);

                var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
                PopulateListView(scriptList, focusedListView, selectedIndices);
            }
        }

        private bool IsThereAnyItemSelected(ListView listView)
        {
            return listView.SelectedItems.Count > 0;
        }

        private bool SaveConfigurationFile<T>(AppData<T> appData, bool askForConfirmation = true) where T : INamed, new()
        {
            bool configurationSaved = false;

            var fileName = appData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName];
            var dbLocation = appData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];

            string fullPath = string.Empty;            

            if (appData.IsNewConfiguration)
            {
                fullPath = CommonDialogs.SaveToFileDialogBox(fileName, dbLocation);
            }
            else
            {
                fullPath = Path.Combine(dbLocation, fileName);
                if (askForConfirmation && CommonDialogs.AreYouSure($"Save file?\n{fullPath}") == DialogResult.No)
                    fullPath = string.Empty;
            }

            if (!string.IsNullOrEmpty(fullPath))
            {
                appData.SaveItems(Path.GetDirectoryName(fullPath), Path.GetFileName(fullPath));
                configurationSaved = true;
            }

            return configurationSaved;
        }

        private void SelectConfigurationFileToOpen<T>(AppData<T> appData, ListView listViewToPopulate) where T : INamed, new()
        {
            var folderToOpen = appData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];

            var fullPath = CommonDialogs.SelectFileDialogBox("", folderToOpen);
            if (!string.IsNullOrEmpty(fullPath))
                OpenConfigurationFile(appData, listViewToPopulate, fullPath);
        }

        private bool OpenConfigurationFile<T>(AppData<T> appData, ListView listViewToPopulate, string fullPath) where T : INamed, new()
        {
            var fileOpened = true;
            try
            {
                appData.Load(fullPath);
                PopulateListView(appData.Instances, listViewToPopulate);
                UpdateListOfLastOpenedFiles();
                UpdateConfigFileListHeader(appData);
            }
            catch (FileNotFoundException ex)
            {
                Trace.WriteLine(ex);
                CommonDialogs.TellUserFileCouldNotBeOpened(fullPath);
                fileOpened = false;
            }

            return fileOpened;
        }

        private void UpdateConfigFileListHeader<T>(AppData<T> appData) where T : INamed, new()
        {
            
            Label listViewHeader = (appData.Equals(_scriptsAppData)) ? lblScriptsTitle : lblDatabasesTitle;
            listViewHeader.Text = listViewHeader.Text.Split(new char[] { ' ' })[0]
                + " ...\\"
                + Path.GetFileName(appData.LastOpenedFiles[0]);
        }

        private bool CheckConfigurationIsSaved<T>(AppData<T> appData, string appDataContentForMessage) where T: INamed, new()
        {
            bool operationResult = true;
            DialogResult dialogResult = DialogResult.OK;

            if (appData.HaveChanged())
            {
                dialogResult = MessageBox.Show($"{appDataContentForMessage.ToUpper()} configuration has changed. Do want to save the current list?", "CONTENT HAVE CHANGED", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                    operationResult = SaveConfigurationFile(appData, false);
            }

            if (dialogResult == DialogResult.Cancel)
            {
                operationResult = false;
            }

            appData.BackupCurrentStatus();
            return operationResult;
        }

        private void UpdateListOfLastOpenedFiles()
        {
            UpdateListOfLastOpenedFiles(_databasesAppData, serversToolStripMenuItem, menuServersSeparator);
            UpdateListOfLastOpenedFiles(_scriptsAppData, scriptsToolStripMenuItem, menuScriptsSeparator);
        }

        private void UpdateListOfLastOpenedFiles<T>(AppData<T> appData, ToolStripMenuItem menuOption, ToolStripSeparator separator) where T : INamed, new()
        {
            RemoveItemsBelowSeparator(menuOption, separator);

            foreach (var fullFileName in appData.LastOpenedFiles)
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = fullFileName;
                menuItem.Click += (sender, e) => { LastOpenedFiles_Click(menuItem, appData); };
                menuOption.DropDownItems.Add(menuItem);
            }
        }
    }
}
