using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunnerLogic.Interfaces
{
    public interface IRepositoryInformation
    {
        string Name { get; set; }
        string Location { get; set; }
    }
}
