using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbScriptRunnerLogic.Entities
{
    public class ArrangeableList<T> : List<T>, IArrangeableList<T>
    {
        public bool ListHasChanged { get; private set; } = false;

        public List<int> MoveItemsUpOnePosition(List<int> indicesToMove)
        {
            var movedIndices = new List<int>();

            for (int i = 0; i < indicesToMove.Count; i++)
            {
                int index = indicesToMove[i];

                if (CanMoveUp(index, indicesToMove))
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

        /// <summary>
        /// Use this method to let the list know that an element has changed
        /// </summary>
        /// <param name="index"></param>
        /// <param name="element"></param>
        public void ReplaceAt(int index, T element)
        {
            this[index] = element;
            this.ListHasChanged = true;
        }

        public new void Add(T element)
        {
            base.Add(element);
            this.ListHasChanged = true;
        }

        public new void AddRange(IEnumerable<T> elements)
        {
            base.AddRange(elements);
            this.ListHasChanged = true;
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            this.ListHasChanged = true;
        }

        public void InitializeWith(IEnumerable<T> elements)
        {
            this.Clear();
            this.AddRange(elements);
            this.ListHasChanged = false;
        }

        public List<int> MoveItemsDownOnePosition(List<int> indicesToMove)
        {
            var movedIndices = new List<int>();

            for (int i = indicesToMove.Count-1; i >= 0; i--)
            {
                int index = indicesToMove[i];

                if (CanMoveDown(index, indicesToMove))
                {
                    MoveItemDownOnePosition(index);
                    movedIndices.Add(index + 1);
                    indicesToMove[i] = index + 1;
                }
                else
                    movedIndices.Add(index);
            }

            return movedIndices;
        }

        public List<int> RemoveAt(List<int> indicesToRemove)
        {
            var removedIndices = new List<int>();

            var maxIndex = this.Count - 1;
            for (var i = 0; i <= indicesToRemove.Count-1; i++)
            {
                int index = indicesToRemove[i];
                if (index >= 0 && index <= maxIndex)
                {
                    this.RemoveAt(index);

                    var removedIndex = index;
                    maxIndex = this.Count - 1;
                    if (removedIndex > maxIndex) removedIndex--;
                    if (removedIndex >= 0 && !removedIndices.Contains(removedIndex)) removedIndices.Add(removedIndex);

                    for (int j = i + 1; j <= indicesToRemove.Count - 1; j++) indicesToRemove[j]--;
                }
            }

            removedIndices.RemoveAll(x => x > maxIndex);

            return removedIndices;
        }

        private void MoveItemUpOnePosition(int indexToMove)
        {
            if (indexToMove < 1) return;

            var item = this[indexToMove];
            this.RemoveAt(indexToMove);
            this.Insert(indexToMove - 1, item);
            this.ListHasChanged = true;
        }

        private void MoveItemDownOnePosition(int indexToMove)
        {
            var maxIndex = this.Count() - 1;
            if (indexToMove == maxIndex) return;

            var item = this[indexToMove];
            this.RemoveAt(indexToMove);
            this.Insert(indexToMove + 1, item);
            this.ListHasChanged = true;
        }

        private bool CanMoveUp(int itemIndex, List<int> indicesToMove)
        {
            bool canMoveUp = (
                MovingMoreThanOne(indicesToMove)
                && !IsTheFirst(itemIndex)
                && !ItemAboveInTheListIsAlsoSelected(itemIndex, indicesToMove)
                );

            canMoveUp = canMoveUp
                ||
                (!IsTheFirst(itemIndex) 
                &&
                (MovingOnlyOne(indicesToMove)
                ||
                (MovingMoreThanOne(indicesToMove) && (IsTheFirst(itemIndex) || (!IsTheFirst(itemIndex) && !ItemAboveInTheListIsAlsoSelected(itemIndex, indicesToMove)))))
                );

            return canMoveUp;
        }

        private bool CanMoveDown(int itemIndex, List<int> indicesToMove)
        {
            bool canMoveDown = (
                MovingMoreThanOne(indicesToMove)
                && !IsTheLast(itemIndex)
                && !ItemBelowInTheListIsAlsoSelected(itemIndex, indicesToMove)
                );

            canMoveDown = canMoveDown
                ||
                (!IsTheLast(itemIndex)
                &&
                (MovingOnlyOne(indicesToMove) || (MovingMoreThanOne(indicesToMove) && !ItemBelowInTheListIsAlsoSelected(itemIndex, indicesToMove)))
                );

            return canMoveDown;
        }

        private bool MovingOnlyOne(List<int> indicesToMove) => indicesToMove.Count == 1;

        private bool MovingMoreThanOne(List<int> indicesToMove) => indicesToMove.Count > 1;

        private bool IsTheFirst(int itemIndex) => itemIndex == 0;

        private bool IsTheLast(int itemIndex) => itemIndex == this.Count - 1;

        private bool ItemAboveInTheListIsAlsoSelected(int itemIndex, List<int> indicesToMove) => indicesToMove.Contains(itemIndex - 1);

        private bool ItemBelowInTheListIsAlsoSelected(int itemIndex, List<int> indicesToMove) => indicesToMove.Contains(itemIndex + 1);
    }
}
