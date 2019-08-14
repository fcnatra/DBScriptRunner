using DbScriptRunnerLogic.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbScriptRunner.UI
{
    public partial class frmAddEditDatabase : Form
    {
        private Database _databaseInformation;

        public Database DatabaseInformation {
            get
            {
                if (_databaseInformation == null) _databaseInformation = new Database();
                return _databaseInformation;
            }

            set
            {
                _databaseInformation = value;
                InitializeDatabaseInformationUI();
            }
        }

        public delegate IEnumerable<string> Delegate_GetErrorsOnDatabaseInformation(Database proposedDatabaseInfo);

        public Delegate_GetErrorsOnDatabaseInformation MethodToGetErrorsOnItemInformation { get; set; }

        public frmAddEditDatabase()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void InitializeDatabaseInformationUI()
        {
            this.txtName.Text = DatabaseInformation.Name;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var databaseInfo = new Database
            {
                Name = this.txtName.Text
            };

            if (MethodToGetErrorsOnItemInformation != null)
            {
                var errorList = MethodToGetErrorsOnItemInformation(databaseInfo);
                if (errorList.Any())
                {
                    var message = string.Empty;
                    foreach (var error in errorList) message += error + Environment.NewLine;
                    MessageBox.Show(message, "INFORMATION");
                    return;
                }
            }
            this.DatabaseInformation = databaseInfo;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
