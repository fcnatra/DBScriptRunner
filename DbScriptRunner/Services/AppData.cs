using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunner.Services
{
    public class AppData
    {
        public ApplicationStatusBackup StatusBackup { get; set; }

        public List<INamed> Databases { get; set; }

        public DatabasePersistence DatabasePersistence { get; set; }

        public void RecoverLastStatus()
        {
            try
            {
                StatusBackup.RecoverStatus();

                var databaseName = "";
                var databaseLocation = "";

                if (StatusBackup.BackupContent.Count > 0)
                {
                    databaseName = StatusBackup.BackupContent[0];
                    databaseLocation = StatusBackup.BackupContent[1];
                    StatusBackup.BackupContent.Clear();
                    LoadDatabases(databaseName, databaseLocation);
                }
            }
            catch
            {
                Debug.WriteLine("Could not load last database status");
            }
        }

        public void LoadDatabases(string databaseName, string databaseLocation)
        {
            DatabasePersistence.Repository.Name = databaseName;
            DatabasePersistence.Repository.Location = databaseLocation;
            Databases = DatabasePersistence.Load();
        }

        public void BackupCurrentStatus()
        {
            StatusBackup.BackupContent.Add(DatabasePersistence.Repository.Name);
            StatusBackup.BackupContent.Add(DatabasePersistence.Repository.Location);
            StatusBackup.SaveStatus();
        }
    }
}
