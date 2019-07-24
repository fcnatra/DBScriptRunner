using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunner.Model
{
    public class StatusBackupElement : INamed
    {
        public string Name { get; set; }
    }
}
