﻿using DbScriptRunner.Services;
using DbScriptRunnerLogic;
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

            PopulateDatabasesListView(_appData.Databases, lvDatabases);
        }

        private void PopulateDatabasesListView(List<INamed> dataSource, ListView listViewm, List<int> indicesToSelect = null)
        {
            lvDatabases.SuspendLayout();

            lvDatabases.Items.Clear();

            if (dataSource != null && dataSource.Any())
            {
                for (int i = 0; i < _appData.Databases.Count - 1; i++)
                {
                    var lvItem = lvDatabases.Items.Add(i.ToString());
                    lvItem.Tag = _appData.Databases[i];
                    lvItem.SubItems.Add(_appData.Databases[i].Name);
                }
                SelectIndicesOnListView(lvDatabases, indicesToSelect);
            }

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

            var indicesToSelect = MoveUpSelectedItemsOnePosition(_appData.Databases, lvDatabases);
            PopulateDatabasesListView(_appData.Databases, lvDatabases, indicesToSelect);
        }

        private void toolbarMoveDown_Click(object sender, EventArgs e)
        {
            if (!IsThereAnyItemSelected(lvDatabases))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }

            var indicesToSelect = MoveDownSelectedItemsOnePosition(_appData.Databases, lvDatabases);
            PopulateDatabasesListView(_appData.Databases, lvDatabases, indicesToSelect);
        }

        private void toolbarRemove_Click(object sender, EventArgs e)
        {
            if (!IsThereAnyItemSelected(lvDatabases))
            {
                CommonDialogs.TellUserToSelectItemsInOrderToMove();
                return;
            }
            var indicesToSelect = RemoveItemsfromListView(_appData.Databases, lvDatabases);
            PopulateDatabasesListView(_appData.Databases, lvDatabases, indicesToSelect);
        }

        private List<int> RemoveItemsfromListView(List<INamed> relatedDataSource, ListView listView)
        {
            var selectedItems = listView.SelectedItems;
            var itemIndex = selectedItems[0].Index;
            var indicesToSelect = new List<int>();
            if (itemIndex == relatedDataSource.Count-1)
                indicesToSelect = new List<int>() { relatedDataSource.Count - 1 };
            else
                indicesToSelect = new List<int>() { selectedItems[0].Index };


            foreach (ListViewItem lvItem in selectedItems)
                relatedDataSource.RemoveAt(lvItem.Index);

            return indicesToSelect;
        }

        private List<int> MoveDownSelectedItemsOnePosition(List<INamed> relatedDataSource, ListView listView)
        {
            var selectedItems = listView.SelectedItems;
            var movedIndices = new List<int>();
            for (var selectedItemIndex = selectedItems.Count-1; selectedItemIndex >= 0; selectedItemIndex--)
            {
                var lvItem = selectedItems[selectedItemIndex];
                if (CanMoveDownOnListView(relatedDataSource, lvItem))
                {
                    relatedDataSource.MoveDownItemOnePosition(lvItem.Index);
                    movedIndices.Add(lvItem.Index + 1);
                }
                else
                    movedIndices.Add(lvItem.Index);
            }
            return movedIndices;
        }

        private List<int> MoveUpSelectedItemsOnePosition(List<INamed> relatedDataSource, ListView listView)
        {
            var selectedItems = listView.SelectedItems;
            var movedIndices = new List<int>();
            foreach (ListViewItem lvItem in selectedItems)
            {
                var lvItemIndex = lvItem.Index;
                if (CanMoveUpOnListView(relatedDataSource, lvItem))
                {
                    relatedDataSource.MoveUpItemOnePosition(lvItemIndex);
                    movedIndices.Add(lvItemIndex - 1);
                }
                else
                    movedIndices.Add(lvItemIndex);
            }
            return movedIndices;
        }

        private bool CanMoveDownOnListView(List<INamed> relatedDataSource, ListViewItem lvItem)
        {
            var selectedItems = lvItem.ListView.SelectedItems;
            var lvIndex = lvItem.Index;
            int selectedItemIndex = selectedItems.IndexOf(lvItem);
            var maxSelectedItemsIndex = selectedItems.Count - 1;
            var maxLvIndex = lvItem.ListView.Items.Count - 1;

            bool canMoveDown = (
                selectedItems.Count > 1 &&
                selectedItemIndex < maxSelectedItemsIndex &&
                relatedDataSource[lvIndex + 1] != selectedItems[selectedItemIndex + 1].Tag
                );

            canMoveDown = canMoveDown ||
                (
                lvIndex < maxLvIndex &&
                (selectedItems.Count == 1 ||
                (selectedItems.Count > 1 &&
                ((selectedItemIndex < maxSelectedItemsIndex && relatedDataSource[lvIndex + 1] != selectedItems[selectedItemIndex + 1].Tag) || selectedItemIndex == maxSelectedItemsIndex)
                )));

            return canMoveDown;
        }

        private bool CanMoveUpOnListView(List<INamed> relatedDataSource, ListViewItem lvItem)
        {
            var selectedItems = lvItem.ListView.SelectedItems;
            var lvIndex = lvItem.Index;
            int selectedItemIndex = selectedItems.IndexOf(lvItem);

            bool canMoveUp = ( 
                selectedItems.Count > 1 &&
                selectedItemIndex > 0 &&
                relatedDataSource[lvIndex - 1] != selectedItems[selectedItemIndex - 1].Tag
                );

            canMoveUp = canMoveUp ||
                (
                lvIndex > 0 &&
                (selectedItems.Count == 1 ||
                (selectedItems.Count > 1 &&
                ((selectedItemIndex > 0 && relatedDataSource[lvIndex - 1] != selectedItems[selectedItemIndex - 1].Tag) || selectedItemIndex == 0)
                ))
                );

            return canMoveUp;
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
                PopulateDatabasesListView(_appData.Databases, lvDatabases);
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
    }
}
