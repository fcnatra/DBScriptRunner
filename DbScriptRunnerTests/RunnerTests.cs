using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Collections.Generic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using DbScriptRunner.Services;
using DbScriptRunnerLogic;

namespace DbScriptRunnerTests
{
    [TestClass]
    public class RunnerTests
    {
        [TestMethod]
        public void RunningScriptsWillCreateOneConnectionPerDatabase()
        {
            var databases = new List<Database> { new Database { Name = "A" }, new Database { Name = "B" } };
            var expectedConnections = databases.Count();
            var connectionsCreated = 0;

            var fakeDbService = A.Fake<IDatabaseService>();
            A.CallTo(() => fakeDbService.CreateDatabaseService()).Invokes(() => connectionsCreated++);

            var runner = new Runner();
            runner.DatabaseServiceFactory = fakeDbService;
            runner.Databases = databases;
            runner.Run();

            Assert.AreEqual(expectedConnections, connectionsCreated);
        }
    }
}

