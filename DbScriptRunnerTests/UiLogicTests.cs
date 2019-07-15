using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Collections.Generic;

namespace DbScriptRunnerTests
{
    [TestClass]
    public class UiLogicTests
    {
        [TestMethod]
        public void PersistedDatabaseListIsParedBackAsExpected()
        {
            var databases = new List<DbScriptRunnerLogic.Entities.Database>()
            {
                new DbScriptRunnerLogic.Entities.Database{Name = "one"},
                new DbScriptRunnerLogic.Entities.Database{Name = "two"}
            };
            var expectedNumberOfDatabases = databases.Count();
            string repositoryContent = "";

            var fakeRepository = A.Fake<DbScriptRunnerLogic.Interfaces.IRepository>();
            fakeRepository.Location = "c:\\temp";
            fakeRepository.Name = "testing.txt";

            var dataPersistence = new DbScriptRunnerLogic.DataPersistence
            {
                Repository = fakeRepository
            };
            var app = new DbScriptRunner.Services.AppData();

            app.Databases = new System.Collections.Generic.List<DbScriptRunnerLogic.Interfaces.INamed>();
            app.Databases.AddRange(databases);

            A.CallTo(() => fakeRepository.Save(A<string>.That.IsNotNull())).Invokes((x) => repositoryContent = (string)x.Arguments.First());

            app.DataPersistence = dataPersistence;
            app.SaveDatabases(fakeRepository.Name, fakeRepository.Location);
            app.BackupCurrentStatus();

            A.CallTo(() => fakeRepository.Load()).Returns(repositoryContent);

            var secondInstanceSameUser = new DbScriptRunner.Services.AppData();
            secondInstanceSameUser.DataPersistence = dataPersistence;
            secondInstanceSameUser.RecoverLastStatus();

            Assert.AreEqual(expectedNumberOfDatabases, app.Databases.Count);
            var expectedDatabasesWereFound = true;
            foreach (var db in app.Databases)
            {
                expectedDatabasesWereFound = databases.Any(expectedDb => expectedDb.Name == db.Name);
                if (!expectedDatabasesWereFound) break;
            }
            Assert.IsTrue(expectedDatabasesWereFound);
        }

        
    }
}

