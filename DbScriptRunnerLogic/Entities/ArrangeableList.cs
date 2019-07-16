using System.Collections.Generic;
using System.Linq;

namespace DbScriptRunnerLogic.Entities
{
    public class ArrangeableList<T> : List<T>
    {
        public bool ListHasChanged { get; private set; }

        public List<int> MoveItemsUpOnePosition(List<int> indicesToMove)
        {
            var movedIndices = new List<int>();

            for (int i = 0; i < indicesToMove.Count; i++)
            {
                int index = indicesToMove[i];

                if (CanMoveUpOnList(index, indicesToMove))
                {
                    MoveItemUpOnePosition(index);
                    movedIndices.Add(index - 1);
                    indicesToMove[i] = index - 1;
                }
                else
                    movedIndices.Add(index);
            }

            return movedIndices;
        }

        public void MoveItemUpOnePosition(int indexToMove)
        {
            if (indexToMove < 1) return;

            var item = this[indexToMove];
            this.RemoveAt(indexToMove);
            this.Insert(indexToMove - 1, item);
            this.ListHasChanged = true;
        }

        public void MoveItemDownOnePosition(int indexToMove)
        {
            var maxIndex = this.Count() - 1;
            if (indexToMove == maxIndex) return;

            var item = this[indexToMove];
            this.RemoveAt(indexToMove);
            this.Insert(indexToMove + 1, item);
            this.ListHasChanged = true;
        }

        public bool IsEmpty()
        {
            return !this.Any();
        }

        private bool CanMoveUpOnList(int itemIndex, List<int> indicesToMove)
        {
            bool canMoveUp = (
                MovingMoreThanOne(indicesToMove)
                && !IsTheFirst(itemIndex)
                && !AboveInTheListIsAlsoSelected(itemIndex, indicesToMove)
                );

            canMoveUp = canMoveUp
                ||
                ( !IsTheFirst(itemIndex) 
                &&
                (MovingOnlyOne(indicesToMove)
                ||
                (MovingMoreThanOne(indicesToMove) && (IsTheFirst(itemIndex) || (!IsTheFirst(itemIndex) && !AboveInTheListIsAlsoSelected(itemIndex, indicesToMove)))))
                );

            return canMoveUp;
        }

        private bool MovingOnlyOne(List<int> indicesToMove) => indicesToMove.Count == 1;

        private bool MovingMoreThanOne(List<int> indicesToMove) => indicesToMove.Count > 1;

        private bool IsTheFirst(int itemIndex) => itemIndex == 0;

        private bool AboveInTheListIsAlsoSelected(int itemIndex, List<int> indicesToMove) => indicesToMove.Contains(itemIndex - 1);
    }
}
