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
        private AppData _appData;        

        public frmMain()
        {
            InitializeComponent();

            ConfigureLogicDependencies();

            _appData.RecoverLastStatus();

            InitializeControlsDesign();
        }

        private void ConfigureLogicDependencies()
        {
            var fileRepository = new TextFileRepository();
            _appData = new AppData
            {
                DataPersistence = new DataPersistence() { Repository = fileRepository },
            };
        }

        private void InitializeControlsDesign()
        {
            lvDatabases.Columns.Add(new ColumnHeader { DisplayIndex = 0, Width = 70, Text = "Order", Name = "lvDatabasesHeaderOrder" });
            lvDatabases.Columns.Add(new ColumnHeader { DisplayIndex = 1, Width = 300, Text = "Name", Name = "lvDatabasesHeaderName" });
            lvDatabases.Columns[0].TextAlign = HorizontalAlignment.Center;
            lvDatabases.View = View.Details;

            PopulateDatabasesListView();
        }

        private void PopulateDatabasesListView(List<int> indicesToSelect = null)
        {
            lvDatabases.Items.Clear();

            var datasource = _appData.Databases;
            if (datasource == null || !datasource.Any()) return;

            lvDatabases.SuspendLayout();
            int index = 0;
            foreach (var database in datasource)
            {
                ListViewItem lvItem = lvDatabases.Items.Add(index.ToString());
                lvItem.Tag = database;
                lvItem.SubItems.Add(database.Name);
                ++index;
            }

            SelectIndicesOnListView(lvDatabases, indicesToSelect);

            lvDatabases.ResumeLayout();
        }

        private void SelectIndicesOnListView(ListView lvControl, List<int> indicesToSelect)
        {
            if (indicesToSelect == null) return;

            var maxIndex = lvControl.Items.Count;
            foreach (var index in indicesToSelect)
                lvControl.SelectedIndices.Add(index);
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
            DialogResult dialogResult = DialogResult.OK;
            if (_appData.DatabasesHaveChanged())
            {
                dialogResult = MessageBox.Show("Databases changed. Want to save the current list?", "CONTENT HAVE CHANGED", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                    SaveDatabases();
            }

            if (dialogResult == DialogResult.Cancel)
                e.Cancel = true;
            else
                _appData.BackupCurrentStatus();
        }

        private void toolbarMoveUp_Click(object sender, EventArgs e)
        {
            if (!IsThereAnyItemSelected(lvDatabases))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }

            var selectedIndices = lvDatabases.SelectedIndices.Cast<int>().ToList();
            var indicesToSelect = ((ArrangeableList<INamed>)_appData.Databases).MoveItemsUpOnePosition(selectedIndices);
            PopulateDatabasesListView(indicesToSelect);
        }

        private void toolbarMoveDown_Click(object sender, EventArgs e)
        {
            if (!IsThereAnyItemSelected(lvDatabases))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }

            var selectedIndices = lvDatabases.SelectedIndices.Cast<int>().ToList();
            var indicesToSelect = ((ArrangeableList<INamed>)_appData.Databases).MoveItemsDownOnePosition(selectedIndices);
            PopulateDatabasesListView(indicesToSelect);
        }

        private void toolbarRemove_Click(object sender, EventArgs e)
        {
            if (!IsThereAnyItemSelected(lvDatabases))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }

            var selectedIndices = lvDatabases.SelectedIndices.Cast<int>().ToList();
            var indicesToSelect = ((ArrangeableList<INamed>)_appData.Databases).RemoveAt(selectedIndices);
            PopulateDatabasesListView(indicesToSelect);
        }

        private void toolbarAddNewItem_Click(object sender, EventArgs e)
        {
            AddNewDatabase();
        }

        private void toolbarEdit_Click(object sender, EventArgs e)
        {
            EditSelectedDatabaseItem();
        }

        private void EditSelectedDatabaseItem()
        {
            if (lvDatabases.SelectedIndices.Count != 1)
            {
                CommonDialogs.TellUserToSelectJustOneItem();
                return;
            }

            var editForm = new frmAddEditDatabase();
            editForm.GetErrorsOnDatabaseInformation = _appData.GetErrorsOnDatabaseInformation;
            var databaseList = ((ArrangeableList<INamed>)_appData.Databases);

            editForm.DatabaseInformation = (Database)databaseList[lvDatabases.SelectedIndices[0]];

            var dialogResult = editForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var selectedIndices = lvDatabases.SelectedIndices.Cast<int>().ToList();
                databaseList.ReplaceAt(lvDatabases.SelectedIndices[0], editForm.DatabaseInformation);
                PopulateDatabasesListView(selectedIndices);
            }
        }

        private void AddNewDatabase()
        {
            var addForm = new frmAddEditDatabase();
            addForm.GetErrorsOnDatabaseInformation = _appData.GetErrorsOnDatabaseInformation;

            var dialogResult = addForm.ShowDialog();
            var databaseList = ((ArrangeableList<INamed>)_appData.Databases);
            if (dialogResult == DialogResult.OK)
            {
                var selectedIndices = lvDatabases.SelectedIndices.Cast<int>().ToList();
                databaseList.Add(addForm.DatabaseInformation);
                PopulateDatabasesListView(selectedIndices);
            }
        }

        private bool IsThereAnyItemSelected(ListView listView)
        {
            return listView.SelectedItems.Count > 0;
        }

        private void menuOpenServersConfiguration_Click(object sender, EventArgs e)
        {
            var destinationInfo = _appData.DatabaseRepositoryInformation;

            var fullPath = CommonDialogs.SelectFileDialogBox("", destinationInfo.Location);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _appData.LoadDatabases(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
                PopulateDatabasesListView();
            }
        }

        private void menuSaveServersConfiguration_Click(object sender, EventArgs e)
        {
            SaveDatabases();
        }

        private void SaveDatabases()
        {
            var destinationInfo = _appData.DatabaseRepositoryInformation;

            var fullPath = CommonDialogs.SaveToFileDialogBox(destinationInfo.Name, destinationInfo.Location);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _appData.SaveDatabases(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
            }
        }

        private void addServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewDatabase();
        }

        private void editSelectedServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedDatabaseItem();
        }
    }
}
