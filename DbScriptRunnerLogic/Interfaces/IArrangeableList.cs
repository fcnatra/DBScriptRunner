
using System.Collections;
using System.Collections.Generic;

namespace DbScriptRunnerLogic.Interfaces
{
    public interface IArrangeableList<T> : IEnumerable<T>
    {
        void InitializeWith(IEnumerable<T> elements);
    }
}
