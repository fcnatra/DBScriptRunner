using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunner.Services
{
    public static class ListExtension
    {
        private static Dictionary<int, bool> _stateHolder = new Dictionary<int, bool>();

        public static void MoveUpItemOnePosition<T>(this List<T> list, int indexToMove)
        {
            if (indexToMove < 1) return;

            var item = list[indexToMove];
            list.RemoveAt(indexToMove);
            list.Insert(indexToMove - 1, item);
            SetHasChanged(list, true);
        }

        public static bool GetHasChanged<T>(this List<T> list)
        {
            if (!_stateHolder.ContainsKey(list.GetHashCode()))
                SetHasChanged(list, false);

            return _stateHolder.ContainsKey(list.GetHashCode());
        }

        public static void SetHasChanged<T>(this List<T> list, bool changed)
        {
            _stateHolder[list.GetHashCode()] = changed;
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return !list.Any();
        }
    }
}
