using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Entities;
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
        public AppData()
        {
            StatusBackup = new ApplicationStatusBackup();
        }

        public ApplicationStatusBackup StatusBackup { get; set; }

        public IEnumerable<INamed> Databases { get; set; }

        public DataPersistence DataPersistence { get; set; }

        public IRepositoryInformation DatabaseRepositoryInformation => DataPersistence?.Repository;

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

        public void LoadDatabases(string locationName, string locationPath)
        {
            DataPersistence.Repository.Name = locationName;
            DataPersistence.Repository.Location = locationPath;
            Databases = DataPersistence.Load();
        }

        public void SaveDatabases(string locationName, string locationPath)
        {
            DataPersistence.Repository.Name = locationName;
            DataPersistence.Repository.Location = locationPath;
            DataPersistence.Items = Databases;
            DataPersistence.Save();
        }

        public void BackupCurrentStatus()
        {
            StatusBackup.BackupContent.Add(DataPersistence.Repository.Name);
            StatusBackup.BackupContent.Add(DataPersistence.Repository.Location);
            StatusBackup.SaveStatus();
        }

        internal bool DatabasesHaveChanged()
        {
            return ((ArrangeableList<INamed>)Databases).ListHasChanged;
        }
    }
}
