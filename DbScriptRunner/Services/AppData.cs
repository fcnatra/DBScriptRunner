using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DbScriptRunner.Services
{
    public class AppData<T> where T : INamed, new()
    {
        private const string DEFAULT_CONFIGURATION_NAME = "NewConfiguration.txt";

        public ApplicationStatusBackup StatusBackup { get; set; }

        public DataPersistence<T> Persistence { get; set; } = new DataPersistence<T>();

        public IArrangeableList<INamed> Instances { get; set; }

        public AppData()
        {
            StatusBackup = new ApplicationStatusBackup();
            StatusBackup.StatusBackupPrefix = typeof(T).ToString();
        }

        public void RecoverLastStatus()
        {
            var configurationFileName = "";
            var configurationFileLocation = "";

            try
            {
                StatusBackup.RecoverStatus();

                if (StatusBackup.BackupContent.Count > 0)
                {
                    configurationFileName = StatusBackup[ApplicationStatusBackup.StatusItem.ConfigurationFileName];
                    configurationFileLocation = StatusBackup[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];

                    StatusBackup.BackupContent.Clear();
                    Load(configurationFileName, configurationFileLocation);
                }
                else
                    Instances = new ArrangeableList<INamed>();
            }
            catch
            {
                Debug.WriteLine($"Could not load last configuration status from backup file. " +
                    $"DATA: Status backup: {StatusBackup.StatusBackupPrefix + StatusBackup.StatusBackupConfigFileName} " +
                    $"Configuration File Location: {configurationFileLocation} " +
                    $" Configuration File Name: {configurationFileName}");

                StatusBackup[ApplicationStatusBackup.StatusItem.ConfigurationFileName] = DEFAULT_CONFIGURATION_NAME;
                StatusBackup[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        public void BackupCurrentStatus()
        {
            if (RepositoryNameOrRepositoryLocationAreEmpty())
                return;

            StatusBackup[ApplicationStatusBackup.StatusItem.ConfigurationFileName] = Persistence.Repository.Name;
            StatusBackup[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation] = Persistence.Repository.Location;
            StatusBackup.SaveStatus();
        }

        private bool RepositoryNameOrRepositoryLocationAreEmpty()
        {
            return string.IsNullOrEmpty(Persistence.Repository.Name) || string.IsNullOrEmpty(Persistence.Repository.Location);
        }
        
        public void Load(string locationName, string locationPath)
        {
            Persistence.Repository.Name = locationName;
            Persistence.Repository.Location = locationPath;
            Instances = new ArrangeableList<INamed>();
            Instances.InitializeWith(Persistence.Load());
        }

        public void SaveItems(string locationName, string locationPath)
        {
            Persistence.Repository.Name = locationName;
            Persistence.Repository.Location = locationPath;
            Persistence.Items = (List<INamed>)Instances;
            Persistence.Save();

            Instances.InitializeStatus();
        }

        public List<string> CheckForErrorsOnName(T InstanceInfo)
        {
            var errorList = new List<string>();

            if (Instances.Any(x => x.Name.ToUpper() == InstanceInfo.Name.ToUpper()))
                errorList.Add("Instance name is already in use. Please use a different name");

            return errorList;
        }

        internal bool HaveChanged()
        {
            return ((ArrangeableList<INamed>)Instances).ListHasChanged;
        }
    }
}
