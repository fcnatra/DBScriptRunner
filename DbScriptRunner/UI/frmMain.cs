using DbScriptRunner.Services;
using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using DbScriptRunnerLogic.Services;
using System;
using System.Collections.Generic;
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

            _databasesAppData.RecoverLastStatus();
            _scriptsAppData.RecoverLastStatus();

            InitializeControlsDesign();
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
            PopulateListView(_databasesAppData.Instances, lvDatabases, null, _databasesAppData.Persistence?.Repository?.Name);

            InitializeUIForScriptsListView();
            PopulateListView(_scriptsAppData.Instances, lvScripts, null, _scriptsAppData.Persistence?.Repository?.Name);
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
            var destinationInfo = _databasesAppData.Persistence.Repository;

            var fullPath = CommonDialogs.SelectFileDialogBox("", destinationInfo.Location);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _databasesAppData.Load(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
                PopulateListView(_databasesAppData.Instances, lvDatabases);
            }
        }

        private void menuSaveServersConfiguration_Click(object sender, EventArgs e)
        {
            SaveDatabases();
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
            var destinationInfo = _scriptsAppData.Persistence.Repository;

            var fullPath = CommonDialogs.SelectFileDialogBox("", destinationInfo.Location);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _scriptsAppData.Load(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
                PopulateListView(_scriptsAppData.Instances, lvScripts);
            }
        }

        private void menuSaveScriptConfiguration_Click(object sender, EventArgs e)
        {
            SaveScripts();
        }

        private ListView GetFocusedListView()
        {
            if (lvDatabases.Focused) return lvDatabases;
            if (lvScripts.Focused) return lvScripts;

            CommonDialogs.TellUserToFocusAListView();

            return null;
        }

        private void PopulateListView(IArrangeableList<INamed> datasource, ListView listView, List<int> indicesToSelect = null, string lvTitle = "")
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

            if (!string.IsNullOrEmpty(lvTitle)) listView.Text = lvTitle.ToUpper();

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
            var focusedListView = GetFocusedListView();
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
            ListView focusedListView = GetFocusedListView();
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
            ListView focusedListView = GetFocusedListView();
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
            ListView focusedListView = GetFocusedListView();
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
            editForm.GetErrorsOnItemInformation = _scriptsAppData.CheckForErrorsOnName;
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
            editForm.GetErrorsOnDatabaseInformation = _databasesAppData.CheckForErrorsOnName;
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
            ListView focusedListView = GetFocusedListView();
            if (focusedListView == null) return;

            if (focusedListView.SelectedIndices.Count != 1)
            {
                CommonDialogs.TellUserToSelectJustOneItem();
                return;
            }

            if (focusedListView == lvDatabases) AddNewDatabaseItem(focusedListView);
            else AddNewScriptItem(focusedListView);
        }

        private void AddNewDatabaseItem(ListView focusedListView)
        {
            var datasource = (ArrangeableList<INamed>)focusedListView.Tag;

            var addForm = new frmAddEditDatabase();
            addForm.GetErrorsOnDatabaseInformation = _databasesAppData.CheckForErrorsOnName;

            var dialogResult = addForm.ShowDialog();
            var databaseList = ((ArrangeableList<INamed>)_databasesAppData.Instances);
            if (dialogResult == DialogResult.OK)
            {
                var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
                databaseList.Add(addForm.DatabaseInformation);
                PopulateListView(datasource, focusedListView, selectedIndices);
            }
        }

        private void AddNewScriptItem(ListView focusedListView)
        {
            var datasource = (ArrangeableList<INamed>)focusedListView.Tag;

            var addForm = new frmAddEditScript();
            addForm.GetErrorsOnItemInformation = _scriptsAppData.CheckForErrorsOnName;

            var dialogResult = addForm.ShowDialog();
            var databaseList = ((ArrangeableList<INamed>)_databasesAppData.Instances);
            if (dialogResult == DialogResult.OK)
            {
                var selectedIndices = focusedListView.SelectedIndices.Cast<int>().ToList();
                databaseList.Add(addForm.ItemInformation);
                PopulateListView(datasource, focusedListView, selectedIndices);
            }
        }

        private bool IsThereAnyItemSelected(ListView listView)
        {
            return listView.SelectedItems.Count > 0;
        }

        private void SaveDatabases()
        {
            var destinationInfo = _databasesAppData.Persistence.Repository;

            var fullPath = CommonDialogs.SaveToFileDialogBox(destinationInfo.Name, destinationInfo.Location);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _databasesAppData.SaveItems(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
            }
        }

        private void SaveScripts()
        {
            var destinationInfo = _scriptsAppData.Persistence.Repository;

            var fullPath = CommonDialogs.SaveToFileDialogBox(destinationInfo.Name, destinationInfo.Location);
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
                    SaveScripts();
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
                    SaveDatabases();
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
