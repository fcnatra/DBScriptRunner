using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Collections.Generic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
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
            A.CallTo(() => fakeDbService.CreateDatabaseService()).Returns(fakeDbService);
            A.CallTo(() => fakeDbService.Connect()).Invokes(() => connectionsCreated++);

            var runner = new Runner();
            runner.DatabaseServiceFactory = fakeDbService;
            runner.Databases = databases;
            runner.Run();

            Assert.AreEqual(expectedConnections, connectionsCreated);
        }

        [TestMethod]
        public void EveryConnectionsHasADisconnectOrder()
        {
            var databases = new List<Database> { new Database { Name = "A" }, new Database { Name = "B" } };
            var connectionsCreated = 0;
            var disconnections = 0;

            var fakeDbService = A.Fake<IDatabaseService>();
            A.CallTo(() => fakeDbService.CreateDatabaseService()).Returns(fakeDbService);
            A.CallTo(() => fakeDbService.Connect()).Invokes(() => connectionsCreated++);
            A.CallTo(() => fakeDbService.Disconnect()).Invokes(() => disconnections++);

            var runner = new Runner();
            runner.DatabaseServiceFactory = fakeDbService;
            runner.Databases = databases;
            runner.Run();

            Assert.AreEqual(connectionsCreated, disconnections);
        }

        [TestMethod]
        public void WhenRunningSequentiallyThereWillBeOnlyOneActiveConnectionAtATime()
        {
            var databases = new List<Database> { new Database { Name = "A" }, new Database { Name = "B" } };
            var expectedConnectionsAtATime = 1;
            var connectionsCreated = 0;
            var maxConnectionsAtATime = 0;

            var fakeDbService = A.Fake<IDatabaseService>();
            A.CallTo(() => fakeDbService.CreateDatabaseService()).Returns(fakeDbService);
            A.CallTo(() => fakeDbService.Connect()).Invokes(() => {
                connectionsCreated++;
                if (connectionsCreated > maxConnectionsAtATime) maxConnectionsAtATime = connectionsCreated;
            });
            A.CallTo(() => fakeDbService.Disconnect()).Invokes(() => connectionsCreated--);

            var runner = new Runner();
            runner.DatabaseServiceFactory = fakeDbService;
            runner.Databases = databases;
            runner.Run();

            Assert.AreEqual(expectedConnectionsAtATime, maxConnectionsAtATime);
        }

        [TestMethod]
        public void WhenRunningOnParallelThereWillBeAsManyActiveConnectionsAsDatabases()
        {
            var databases = new List<Database> { new Database { Name = "A" }, new Database { Name = "B" } };
            var expectedConnectionsAtATime = databases.Count;
            var connectionsCreated = 0;
            var maxConnectionsAtATime = 0;

            var fakeDbService = A.Fake<IDatabaseService>();
            A.CallTo(() => fakeDbService.CreateDatabaseService()).Returns(fakeDbService);
            A.CallTo(() => fakeDbService.Connect()).Invokes(() => {
                connectionsCreated++;
                if (connectionsCreated > maxConnectionsAtATime) maxConnectionsAtATime = connectionsCreated;
            });
            A.CallTo(() => fakeDbService.Disconnect()).Invokes(() => connectionsCreated--);

            var runner = new Runner();
            runner.DatabaseServiceFactory = fakeDbService;
            runner.Databases = databases;
            runner.ParallelExecution = true;
            runner.Run();

            Assert.AreEqual(expectedConnectionsAtATime, maxConnectionsAtATime);
        }
    }
}

