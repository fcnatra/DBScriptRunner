using DbScriptRunnerLogic.Interfaces;
using System;

namespace DbScriptRunnerLogic.Entities
{
    [Serializable()]
    public class Script : INamed
    {
        public string Name { get; set; } = string.Empty;        
    }
}
