using DbScriptRunnerLogic.Interfaces;
using System;

namespace DbScriptRunnerLogic.Entities
{
    [Serializable()]
    public class Database : INamed, ITargetOfOperations
    {
        public string Name { get; set; } = string.Empty;

        public Enums.OperationResult OperationResult { get; set; } = Enums.OperationResult.None;
    }
}
