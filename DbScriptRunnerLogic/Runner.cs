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

        public List<ExecutionResult> ExecutionResultSummary { get; set; } = new List<ExecutionResult>();

        public void Run()
        {
            foreach (var database in Databases)
            {
                var databaseService = DatabaseServiceFactory.CreateDatabaseService();
                databaseService.Connect();
                databaseService.DatabaseInformation = database;
                databaseService.Disconnect();
            }
        }
    }
}
