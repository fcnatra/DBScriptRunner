using DbScriptRunnerLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DbScriptRunner.UI
{
    public partial class frmAddEditScript : Form
    {
        private Script _itemInformation;

        public Script ItemInformation {
            get
            {
                if (_itemInformation == null) _itemInformation = new Script();
                return _itemInformation;
            }

            set
            {
                _itemInformation = value;
                InitializeDatabaseInformationUI();
            }
        }

        public delegate IEnumerable<string> Delegate_GetErrorsOnItemInformation(Script proposedDatabaseInfo);

        public Delegate_GetErrorsOnItemInformation GetErrorsOnItemInformation { get; set; }

        public frmAddEditScript()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void InitializeDatabaseInformationUI()
        {
            this.txtName.Text = ItemInformation.Name;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var databaseInfo = new Script
            {
                Name = this.txtName.Text
            };

            if (GetErrorsOnItemInformation != null)
            {
                var errorList = GetErrorsOnItemInformation(databaseInfo);
                if (errorList.Any())
                {
                    var message = string.Empty;
                    foreach (var error in errorList) message += error + Environment.NewLine;
                    MessageBox.Show(message, "INFORMATION");
                    return;
                }
            }
            this.ItemInformation = databaseInfo;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            var fullPath = CommonDialogs.SelectFileDialogBox("");
            if (!string.IsNullOrEmpty(fullPath))
            {
                this.txtName.Text = fullPath;
            }
        }
    }
}
