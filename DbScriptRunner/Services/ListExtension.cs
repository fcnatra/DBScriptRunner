using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScriptRunner.Services
{
    public static class ListExtension
    {
        public static void MoveUpItemOnePosition<T>(this List<T> list, int indexToMove)
        {
            if (indexToMove < 1) return;

            var item = list[indexToMove];
            list.RemoveAt(indexToMove);
            list.Insert(indexToMove - 1, item);
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return !list.Any();
        }
    }
}
