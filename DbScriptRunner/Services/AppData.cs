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
    public class AppData<T> where T : INamed, new()
    {
        public AppData()
        {
            StatusBackup = new ApplicationStatusBackup();
        }

        public ApplicationStatusBackup StatusBackup { get; set; }

        public void RecoverLastStatus()
        {
            try
            {
                StatusBackup.RecoverStatus();

                var dbConfigurationFileName = "";
                var dbConfigurationFileLocation = "";

                if (StatusBackup.BackupContent.Count > 0)
                {
                    dbConfigurationFileName = StatusBackup.BackupContent[0];
                    dbConfigurationFileLocation = StatusBackup.BackupContent[1];

                    StatusBackup.BackupContent.Clear();
                    Load(dbConfigurationFileName, dbConfigurationFileLocation);
                }
            }
            catch
            {
                Debug.WriteLine("Could not load last database status");
            }
        }

        public void BackupCurrentStatus()
        {
            StatusBackup.BackupContent.Add(Persistence.Repository.Name);
            StatusBackup.BackupContent.Add(Persistence.Repository.Location);
            StatusBackup.SaveStatus();
        }

        public DataPersistence<T> Persistence { get; set; } = new DataPersistence<T>();

        public IRepositoryInformation RepositoryInformation => Persistence?.Repository;

        public IArrangeableList<INamed> Instances { get; set; }

        public void Load(string locationName, string locationPath)
        {
            Persistence.Repository.Name = locationName;
            Persistence.Repository.Location = locationPath;
            Instances = Persistence.Load();
        }

        public void Save(string locationName, string locationPath)
        {
            Persistence.Repository.Name = locationName;
            Persistence.Repository.Location = locationPath;
            Persistence.Items = Instances;
            Persistence.Save();
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
