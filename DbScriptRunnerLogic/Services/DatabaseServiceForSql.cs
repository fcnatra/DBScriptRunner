using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunnerLogic.Services
{
    public class DatabaseServiceForSql : IDatabaseService
    {
        public Database DatabaseInformation { get; set; }

        public IDbConnection ActiveConnection { get; private set; }

        public IDbConnection Connect()
        {
            throw new NotImplementedException();
        }

        public IDatabaseService CreateDatabaseService()
        {
            return new DatabaseServiceForSql();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
