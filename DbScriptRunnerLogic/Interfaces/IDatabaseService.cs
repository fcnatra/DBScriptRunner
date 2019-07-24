using DbScriptRunnerLogic.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunnerLogic.Interfaces
{
    public interface IDatabaseService
    {
        IDatabaseService CreateDatabaseService();

        Database DatabaseInformation { get; set; }

        IDbConnection ActiveConnection { get; }

        IDbConnection Connect();

        void Disconnect();
    }
}
