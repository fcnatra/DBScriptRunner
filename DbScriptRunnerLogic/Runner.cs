using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunnerLogic
{
    public class Runner
    {
        private bool _stopOnAllServers;

        public enum ErrorStrategy
        {
            DontStop,
            StopOnTheServerWhereTheErrorTookPlace,
            StopOnAllServers,
        }

        public IEnumerable<Database> Databases { get; set; }

        public IEnumerable<Script> Scripts { get; set; }

        public struct ExecutionResult
        {
            public Database database;
            public Script script;
            public string operationResult;
            public string operationMessage;
        }

        public IDatabaseService DatabaseServiceFactory { get; set; }

        public bool ParallelExecution { get; set; } = false;

        public List<ExecutionResult> ExecutionResultSummary { get; set; } = new List<ExecutionResult>();

        public void Run()
        {
            if (ParallelExecution)
                RunParallel();
            else
                RunSequentially();
        }

        private void RunSequentially()
        {
            foreach (var database in Databases) 
                RunOnServer(database);
        }

        private void RunParallel()
        {
            Parallel.ForEach(Databases, (database) =>
            {
                RunOnServer(database);
            });
        }

        private void RunOnServer(Database database)
        {
            var databaseService = DatabaseServiceFactory.CreateDatabaseService();
            databaseService.DatabaseInformation = database;
            databaseService.Connect();

            databaseService.Disconnect();
        }
    }
}
