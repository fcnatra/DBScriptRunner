using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Interfaces;
using DbScriptRunnerLogic.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DbScriptRunner
{
    public partial class frmMain : Form
    {
        private List<INamedEntity> _databases;
        private DatabasePersistence _databasePersistence;
        

        public frmMain()
        {
            InitializeComponent();

            ConfigureLogicDependencies();

            RecoverLastState();

            InitializeControlsDesign();
        }

        private void ConfigureLogicDependencies()
        {
            var fileRepository = new TextFileRepository
            {
                Name = "BigBangTheory.txt",
                Location = "C:\\TEMP"
            };
            _databasePersistence = new DatabasePersistence() { Repository = fileRepository };
        }

        private void InitializeControlsDesign()
        {
            lvDatabases.Columns.Add(new ColumnHeader { DisplayIndex = 0, Width = 70, Text = "Order", Name = "lvDatabasesHeaderOrder"});
            lvDatabases.Columns.Add(new ColumnHeader { DisplayIndex = 1, Width = 300, Text = "Name", Name = "lvDatabasesHeaderName" });
            lvDatabases.Columns[0].TextAlign = HorizontalAlignment.Center;
            lvDatabases.View = View.Details;

            PopulateDatabasesListView();
        }

        private void PopulateDatabasesListView(List<int> previouslySelectedIndices = null)
        {
            lvDatabases.SuspendLayout();

            lvDatabases.Items.Clear();
            for (int i = 0; i < _databases.Count - 1; i++)
            {
                var lvItem = lvDatabases.Items.Add(i.ToString());
                lvItem.Tag = _databases[i];
                lvItem.SubItems.Add(_databases[i].Name);
            }
            SelectIndicesOnListView(lvDatabases, previouslySelectedIndices);

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

        private void RecoverLastState()
        {
            _databases = _databasePersistence.Load();
        }

        private void SaveCurrentState()
        {
            _databasePersistence.Save();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveCurrentState();
        }

        private void toolStripButtonMoveUp_Click(object sender, EventArgs e)
        {
            if (ThereAreNotDatabasesSelected())
            {
                TellUserToSelectItemsInOrderToMove();
                return;
            }

            var newIndicesToSelect = MoveUpSelectedDatabaseItemsOnePosition();
            PopulateDatabasesListView(newIndicesToSelect);
        }

        private List<int> MoveUpSelectedDatabaseItemsOnePosition()
        {
            var selectedItems = lvDatabases.SelectedItems;
            var movedIndices = new List<int>();
            foreach (ListViewItem lvItem in selectedItems)
            {
                var lvItemIndex = lvItem.Index;
                if (CanMoveUpOnListView(lvItem, selectedItems))
                {
                    MoveUpDatabaseItemOnePosition(lvItemIndex);
                    movedIndices.Add(lvItemIndex-1);
                }
                else
                    movedIndices.Add(lvItemIndex);
            }
            return movedIndices;
        }

        private void MoveUpDatabaseItemOnePosition(int indexToMove)
        {
            var item = _databases[indexToMove];
            _databases.RemoveAt(indexToMove);
            _databases.Insert(indexToMove - 1, item);
        }

        private bool CanMoveUpOnListView(ListViewItem lvItem, ListView.SelectedListViewItemCollection selectedItems)
        {
            var lvIndex = lvItem.Index;
            int selectedItemIndex = selectedItems.IndexOf(lvItem);

            bool canMoveUp =
                (
                selectedItems.Count > 1 &&
                selectedItemIndex > 0 &&
                _databases[lvIndex - 1] != selectedItems[selectedItemIndex - 1].Tag
                );
            canMoveUp = canMoveUp ||
                (
                lvIndex > 0 &&
                (selectedItems.Count == 1 || 
                (selectedItems.Count > 1 &&
                ((selectedItemIndex > 0 && _databases[lvIndex - 1] != selectedItems[selectedItemIndex - 1].Tag) || selectedItemIndex == 0)
                ))
                );

            return canMoveUp;
        }

        private bool ThereAreNotDatabasesSelected()
        {
            return lvDatabases.SelectedItems.Count <= 0;
        }

        private void TellUserToSelectItemsInOrderToMove()
        {
            MessageBox.Show("Select items from the list in order to operate with them", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
