using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Collections.Generic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using DbScriptRunner.Services;

namespace DbScriptRunnerTests
{
    [TestClass]
    public class AppDataTests
    {
        [TestMethod]
        public void GivenAPrefixItIsUsedToLoadTheAppDataFile()
        {
            var expectedPrefix = "XYZ";

            var fakeRepository = A.Fake<IRepository>();

            var statusBackup = new ApplicationStatusBackup();
            statusBackup.Persistence.Repository = fakeRepository;
            statusBackup.StatusBackupPrefix = expectedPrefix;
            statusBackup.StatusBackupConfigFileName = "A";
            statusBackup.BackupContent.Add("one");
            statusBackup.BackupContent.Add("two");
            statusBackup.RecoverStatus();

            Assert.IsTrue(fakeRepository.Name.Contains(expectedPrefix));
        }

        [TestMethod]
        public void FileNameUsedToSaveAndLoadAppStatusDataIsTheSame()
        {
            var expectedPrefix = "XYZ";

            var fakeRepository = A.Fake<IRepository>();

            var statusBackup = new ApplicationStatusBackup();
            statusBackup.Persistence.Repository = fakeRepository;
            statusBackup.StatusBackupPrefix = expectedPrefix;
            statusBackup.StatusBackupConfigFileName = "A";
            statusBackup.BackupContent.Add("one");
            statusBackup.BackupContent.Add("two");
            statusBackup.RecoverStatus();

            var expectedFileName = fakeRepository.Name;

            statusBackup.SaveStatus();

            Assert.AreEqual(expectedFileName, fakeRepository.Name);
        }

        [TestMethod]
        public void GivenAPersistedDatabaseList_WhenLoadingListBack_ListIsTheSame()
        {
            // GIVEN
            var databases = (IArrangeableList<INamed>)new ArrangeableList<INamed>()
            {
                new Database {Name = "one"},
                new Database {Name = "two"}
            };
            var expectedNumberOfDatabases = databases.Count();
            string repositoryContent = "";

            var fakeRepository = A.Fake<IRepository>();
            fakeRepository.Location = "c:\\temp";
            fakeRepository.Name = "testing.txt";

            A.CallTo(() => fakeRepository.Save(A<string>.That.IsNotNull()))
                .Invokes((x) => repositoryContent = (string)x.Arguments.First());

            var dbAppConfigData = new DbScriptRunner.Services.AppData<Database>();

            dbAppConfigData.Instances = databases;
            dbAppConfigData.Persistence.Repository = fakeRepository;
            dbAppConfigData.SaveItems(fakeRepository.Name, fakeRepository.Location);
            dbAppConfigData.BackupCurrentStatus();

            A.CallTo(() => fakeRepository.Load()).Returns(repositoryContent);

            // WHEN
            var secondInstanceSameUser = new DbScriptRunner.Services.AppData<Database>();
            secondInstanceSameUser.Persistence.Repository = fakeRepository;
            secondInstanceSameUser.RecoverLastStatus();

            // THEN
            Assert.AreEqual(expectedNumberOfDatabases, dbAppConfigData.Instances.Count());

            var expectedDatabasesWereFound = true;
            foreach (var db in dbAppConfigData.Instances)
            {
                expectedDatabasesWereFound = databases.Any(expectedDb => expectedDb.Name == db.Name);
                if (!expectedDatabasesWereFound) break;
            }

            Assert.IsTrue(expectedDatabasesWereFound);
        }
    }
}

