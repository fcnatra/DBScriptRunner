using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunnerLogic.Entities
{
    [Serializable()]
    public class Database : INamedEntity
    {
        public string Name { get; set; }
    }
}
