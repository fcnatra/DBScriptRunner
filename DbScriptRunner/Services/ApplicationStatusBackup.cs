using DbScriptRunner.Model;
using DbScriptRunnerLogic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using DbScriptRunnerLogic.Services;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace DbScriptRunner.Services
{
    public class ApplicationStatusBackup
    {
        public enum StatusItem
        {
            ConfigurationFileName = 0,
            ConfigurationFileLocation = 1,
        }
        
        public DataPersistence<StatusBackupElement> Persistence { get; set; } = new DataPersistence<StatusBackupElement>();

        public string StatusBackupPrefix { get; set; } = string.Empty;

        public string StatusBackupConfigFileName { get; set; } = "AppStatusBackup.txt";

        public string StatusBackupLastOpenedFiles { get; set; } = "LastOpenedFiles.txt";

        public List<string> BackupContent { get; set; } = new List<string>();

        public string GetWithStatusBackupPrefix(string text) => string.IsNullOrEmpty(StatusBackupPrefix) ? text : $"{StatusBackupPrefix}.{text}";
        
        public string this[StatusItem statusItem]
        {
            get
            {
                return BackupContent[(int)statusItem];
            }

            set
            {
                if (!BackupContent.Any()) BackupContent.AddRange(Enumerable.Repeat(string.Empty, Enum.GetNames(typeof(StatusItem)).Count()).ToList());

                BackupContent[(int)statusItem] = value;
            }
        }

        public ApplicationStatusBackup()
        {
            this.Persistence.Repository = new IsolatedStorageRepository();
        }

        public void SaveStatus()
        {
            Persistence.Items = new List<INamed>();
            Persistence.Items.AddRange(BackupContent.Select(x => new StatusBackupElement { Name = x }));
            Persistence.Repository.Name = GetWithStatusBackupPrefix(StatusBackupConfigFileName);
            Persistence.Save();
        }

        public void RecoverStatus()
        {
            Persistence.Repository.Name = GetWithStatusBackupPrefix(StatusBackupConfigFileName);
            var rowsInContent = Persistence.Load();
            BackupContent.Clear();
            BackupContent.AddRange(rowsInContent.Select(x => x.Name));
        }

        public IEnumerable<string> RecoverLastOpenedFiles()
        {
            Persistence.Repository.Name = GetWithStatusBackupPrefix(StatusBackupLastOpenedFiles);
            var rowsInContent = Persistence.Load();
            return rowsInContent.Select(x => x.Name);
        }

        public void SaveLastOpenedFiles(IEnumerable<string> fileList)
        {
            Persistence.Items = new List<INamed>();
            Persistence.Items.AddRange(fileList.Select(x => new StatusBackupElement { Name = x }));
            Persistence.Repository.Name = GetWithStatusBackupPrefix(StatusBackupLastOpenedFiles);
            Persistence.Save();
        }
    }
}
