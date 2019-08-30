using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DbScriptRunner.Services
{
    public class AppData<T> where T : INamed, new()
    {
        private const string DEFAULT_CONFIGURATION_NAME = "NewConfiguration.txt";
        private const byte MAX_LAST_OPENED_FILES = 5;

        public ApplicationStatusBackup Status { get; set; }

        public DataPersistence<T> Persistence { get; set; }

        public IArrangeableList<INamed> Instances { get; set; }

        public bool IsNewConfiguration => Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName] == DEFAULT_CONFIGURATION_NAME;

        private List<string> _lastOpenedFiles = new List<string>();
        public List<string> LastOpenedFiles => _lastOpenedFiles.ToList();

        public AppData()
        {
            Initialize();
            ClearContent();
        }

        private void Initialize()
        {
            Persistence = new DataPersistence<T>();
        }

        private void ClearContent()
        {
            Instances = new ArrangeableList<INamed>();
            InitializeStatus();
        }

        private void InitializeStatus()
        {
            Status = new ApplicationStatusBackup();
            Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName] = DEFAULT_CONFIGURATION_NAME;
            Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation] = ".\\";
            Status.StatusBackupPrefix = typeof(T).ToString();
        }

        public void RecoverLastStatus()
        {
            var configurationFileName = "";
            var configurationFileLocation = "";

            try
            {
                Status.RecoverStatus();

                if (Status.BackupContent.Count > 0)
                {
                    configurationFileName = Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName];
                    configurationFileLocation = Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];

                    Load(configurationFileLocation, configurationFileName);
                }
                else
                    Instances = new ArrangeableList<INamed>();
            }
            catch
            {
                Debug.WriteLine($"Could not load last configuration status from backup file. " +
                    $"DATA: Status backup: {Status.StatusBackupPrefix + Status.StatusBackupConfigFileName} " +
                    $"Configuration File Location: {configurationFileLocation} " +
                    $" Configuration File Name: {configurationFileName}");

                Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName] = DEFAULT_CONFIGURATION_NAME;
                Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        public void BackupCurrentStatus()
        {
            if (RepositoryNameOrRepositoryLocationAreEmpty())
                return;

            Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName] = Persistence.Repository.Name;
            Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation] = Persistence.Repository.Location;
            Status.SaveStatus();
        }

        private bool RepositoryNameOrRepositoryLocationAreEmpty()
        {
            return string.IsNullOrEmpty(Persistence.Repository.Name) || string.IsNullOrEmpty(Persistence.Repository.Location);
        }
        
        public void Load(string locationPath, string fileName)
        {
            List<INamed> dataForInstances = Persistence.Load(locationPath, fileName).ToList();

            Instances.InitializeWith(dataForInstances);

            UpdateStatusWithLoadedFileInfo(locationPath, fileName);
            UpdateLastOpenedFilesList(locationPath, fileName);
        }

        private void UpdateStatusWithLoadedFileInfo(string locationPath, string fileName)
        {
            Status[ApplicationStatusBackup.StatusItem.ConfigurationFileName] = fileName;
            Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation] = locationPath;
        }

        private void UpdateLastOpenedFilesList(string path, string fileName)
        {
            var fullFileName = Path.Combine(path, fileName);

            RemoveFileName_FromLastOpenedFiles(fullFileName);
            _lastOpenedFiles.Insert(0, fullFileName);
            CutLastOpenedFilesList_ToMatchMaxLimit();
        }

        private void CutLastOpenedFilesList_ToMatchMaxLimit()
        {
            if (_lastOpenedFiles.Count > MAX_LAST_OPENED_FILES)
                _lastOpenedFiles.RemoveAt(MAX_LAST_OPENED_FILES);
        }

        private void RemoveFileName_FromLastOpenedFiles(string fullFileName)
        {
            if (_lastOpenedFiles.Contains(fullFileName)) _lastOpenedFiles.Remove(fullFileName);
        }

        public void Load(string fullFileName)
        {
            Load(Path.GetDirectoryName(fullFileName), Path.GetFileName(fullFileName));
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

        public void CreateNew()
        {
            this.ClearContent();
        }

        public AppData<T> CreateNewFromThis()
        {
            var newAppData = new AppData<T>();
            newAppData.Instances = this.Instances;
            newAppData.Persistence = this.Persistence;
            newAppData.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation] = this.Status[ApplicationStatusBackup.StatusItem.ConfigurationFileLocation];
            return newAppData;
        }
    }
}
