using DbScriptRunnerLogic.Interfaces;
using System;

namespace DbScriptRunnerLogic.Entities
{
    [Serializable()]
    public class Database : INamed, IRunnerTarget
    {
        public string Name { get; set; }

        public Enums.OperationResult OperationResult { get; set; }
    }
}
