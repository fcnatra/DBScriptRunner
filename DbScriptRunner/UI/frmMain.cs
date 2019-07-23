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

        public frmMain()
        {
            InitializeComponent();

            ConfigureLogicDependencies();

            _databasesAppData.RecoverLastStatus();

            InitializeControlsDesign();
        }

        private void ConfigureLogicDependencies()
        {
            var fileRepository = new TextFileRepository();
            _databasesAppData = new AppData<Database>();
            _databasesAppData.Persistence.Repository = fileRepository;
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

            var datasource = _databasesAppData.Instances;
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

            if (!string.IsNullOrEmpty(_databasesAppData.RepositoryInformation?.Name))
                lblDatabasesTitle.Text = _databasesAppData.RepositoryInformation.Name.ToUpper();

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
            if (_databasesAppData.HaveChanged())
            {
                dialogResult = MessageBox.Show("Databases changed. Want to save the current list?", "CONTENT HAVE CHANGED", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                    SaveDatabases();
            }

            if (dialogResult == DialogResult.Cancel)
                e.Cancel = true;
            else
                _databasesAppData.BackupCurrentStatus();
        }

        private void toolbarMoveUp_Click(object sender, EventArgs e)
        {
            if (!IsThereAnyItemSelected(lvDatabases))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }

            var selectedIndices = lvDatabases.SelectedIndices.Cast<int>().ToList();
            var indicesToSelect = ((ArrangeableList<INamed>)_databasesAppData.Instances).MoveItemsUpOnePosition(selectedIndices);
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
            var indicesToSelect = ((ArrangeableList<INamed>)_databasesAppData.Instances).MoveItemsDownOnePosition(selectedIndices);
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
            var indicesToSelect = ((ArrangeableList<INamed>)_databasesAppData.Instances).RemoveAt(selectedIndices);
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
            editForm.GetErrorsOnDatabaseInformation = _databasesAppData.CheckForErrorsOnName;
            var databaseList = ((ArrangeableList<INamed>)_databasesAppData.Instances);

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
            addForm.GetErrorsOnDatabaseInformation = _databasesAppData.CheckForErrorsOnName;

            var dialogResult = addForm.ShowDialog();
            var databaseList = ((ArrangeableList<INamed>)_databasesAppData.Instances);
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
            var destinationInfo = _databasesAppData.RepositoryInformation;

            var fullPath = CommonDialogs.SelectFileDialogBox("", destinationInfo.Location);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _databasesAppData.Load(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
                PopulateDatabasesListView();
            }
        }

        private void menuSaveServersConfiguration_Click(object sender, EventArgs e)
        {
            SaveDatabases();
        }

        private void SaveDatabases()
        {
            var destinationInfo = _databasesAppData.RepositoryInformation;

            var fullPath = CommonDialogs.SaveToFileDialogBox(destinationInfo.Name, destinationInfo.Location);
            if (!string.IsNullOrEmpty(fullPath))
            {
                _databasesAppData.Save(Path.GetFileName(fullPath), Path.GetDirectoryName(fullPath));
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

        private void menuOpenScriptConfiguration_Click(object sender, EventArgs e)
        {

        }
    }
}
