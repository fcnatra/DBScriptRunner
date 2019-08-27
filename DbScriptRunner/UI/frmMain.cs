using DbScriptRunner.Services;
using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using DbScriptRunnerLogic.Services;
using System;
using System.Collections.Generic;
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

            InitializeUIForScriptsListView();
            PopulateListView(_scriptsAppData.Instances, lvScripts, null);

            HighlightListViewHeader(lvDatabases);
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
            CheckDatabaseConfigIsSaved(e);
            if (e.Cancel) return;

            CheckScriptConfigIsSaved(e);
        }

        private void toolbarMoveUp_Click(object sender, EventArgs e)
        {
            MoveItemUp();
        }

        private void toolbarMoveDown_Click(object sender, EventArgs e)
        {
            MoveItemDown();
        }

        private void toolbarRemove_Click(object sender, EventArgs e)
        {
            RemoveItem();
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
            var folderToOpen = _databasesAppData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];

            var fullPath = CommonDialogs.SelectFileDialogBox("", folderToOpen);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _databasesAppData.Load(fullPath);
                PopulateListView(_databasesAppData.Instances, lvDatabases);
            }
        }

        private void menuSaveServersConfiguration_Click(object sender, EventArgs e)
        {
            SaveDatabaseConfigurationFile();
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
            LoadScriptConfigurationFile();
        }

        private void menuSaveScriptConfiguration_Click(object sender, EventArgs e)
        {
            SaveScriptConfigurationFile();
        }

        private void menuLoadScript_Click(object sender, EventArgs e)
        {
            AddNewItem();
        }

        private void menuUnloadScript_Click(object sender, EventArgs e)
        {
            RemoveItem();
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

        private void RemoveItem()
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

        private void MoveItemDown()
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

        private void MoveItemUp()
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

        private void SaveDatabaseConfigurationFile()
        {
            var dbName = _databasesAppData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName];
            var dbLocation = _databasesAppData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];

            string fullPath = string.Empty;

            if (string.IsNullOrEmpty(dbName))
            {
                fullPath = CommonDialogs.SaveToFileDialogBox(dbName, dbLocation);
            }
            else
            {
                if (CommonDialogs.AreYouSure("SAVE FILE") == DialogResult.Yes)
                    fullPath = Path.Combine(dbLocation, dbName);
            }

            if (!string.IsNullOrEmpty(fullPath))
            {
                _databasesAppData.SaveItems(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
            }
        }

        private void LoadScriptConfigurationFile()
        {
            var folderToOpen = _scriptsAppData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];

            var fullPath = CommonDialogs.SelectFileDialogBox("", folderToOpen);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _scriptsAppData.Load(fullPath);
                PopulateListView(_scriptsAppData.Instances, lvScripts);
            }
        }

        private void SaveScriptConfigurationFile()
        {
            var scriptName = _scriptsAppData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName];
            var scriptLocation = _scriptsAppData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];

            var fullPath = CommonDialogs.SaveToFileDialogBox(scriptName, scriptLocation);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _scriptsAppData.SaveItems(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
            }
        }

        private void CheckScriptConfigIsSaved(FormClosingEventArgs e)
        {
            DialogResult dialogResult = DialogResult.OK;
            if (_scriptsAppData.HaveChanged())
            {
                dialogResult = MessageBox.Show("Script configuration has changed. Want to save the current list?", "CONTENT HAVE CHANGED", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                    SaveScriptConfigurationFile();
            }

            if (dialogResult == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            _scriptsAppData.BackupCurrentStatus();
        }

        private void CheckDatabaseConfigIsSaved(FormClosingEventArgs e)
        {
            DialogResult dialogResult = DialogResult.OK;
            if (_databasesAppData.HaveChanged())
            {
                dialogResult = MessageBox.Show("Databases changed. Want to save the current list?", "CONTENT HAVE CHANGED", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                    SaveDatabaseConfigurationFile();
            }

            if (dialogResult == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            _databasesAppData.BackupCurrentStatus();
        }
    }
}
