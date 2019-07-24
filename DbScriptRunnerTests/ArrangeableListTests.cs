using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Collections.Generic;
using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;

namespace DbScriptRunnerTests
{
    [TestClass]
    public class ArrangeableListTests
    {
        private static IEnumerable<object[]> _dataSetsForMoveUpOps => new List<object[]>
        {
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 1 }, new List<string>{"2", "1", "3"},
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0 }, new List<string>{"1", "2", "3"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 2 }, new List<string>{"1", "3", "2"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0, 2 }, new List<string>{"1", "3", "2"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 1, 2 }, new List<string>{"2", "3", "1"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0, 1 }, new List<string>{"1", "2", "3"}
            },
        };

        private static IEnumerable<object[]> _dataSetsForMoveDownOps => new List<object[]>
        {
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 1 }, new List<string>{"1", "3", "2"},
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0 }, new List<string>{"2", "1", "3"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 2 }, new List<string>{"1", "2", "3"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0, 2 }, new List<string>{"2", "1", "3"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 1, 2 }, new List<string>{"1", "2", "3"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0, 1 }, new List<string>{"3", "1", "2"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3", "4", "5", "6"},
                new List<int> { 1, 3, 4 },
                new List<string>{"1", "3", "2", "6", "4", "5"}
            },
        };

        private static IEnumerable<object[]> _dataSetsForRemoveOps => new List<object[]>
        {
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 1 }, new List<string>{"1", "3"},
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0 }, new List<string>{"2", "3"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 2 }, new List<string>{"1", "2"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0, 2 }, new List<string>{"2"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 1, 2 }, new List<string>{"1"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3"}, new List<int> { 0, 1 }, new List<string>{"3"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2", "3", "4", "5", "6"},
                new List<int> { 1, 3, 4 },
                new List<string>{"1", "3", "6"}
            },
            new object[]
            {
                new ArrangeableList<string> {"1", "2"},
                new List<int> {0, 1},
                new List<string>{}
            },
            new object[]
            {
                new ArrangeableList<string> {"1"},
                new List<int> {0},
                new List<string>{}
            },
        };

        [DataTestMethod]
        [DynamicData(nameof(_dataSetsForMoveUpOps))]
        public void GivenAnInputList_MovingUpSelectedIndices_WillEndUpOnExpectedList(ArrangeableList<string> inputList, List<int> indicesToMove, List<string> expectedList)
        {
            var movedIndices = inputList.MoveItemsUpOnePosition(indicesToMove);

            var movedAsExpected = !movedIndices
                .Select(x => expectedList[x] == inputList[x])
                .Any(x => x == false);

            Assert.IsTrue(movedAsExpected);
        }

        [DataTestMethod]
        [DynamicData(nameof(_dataSetsForMoveDownOps))]
        public void GivenAnInputList_MovingDownSelectedIndices_WillEndUpOnExpectedList(ArrangeableList<string> inputList, List<int> indicesToMove, List<string> expectedList)
        {
            var movedIndices = inputList.MoveItemsDownOnePosition(indicesToMove);

            var movedAsExpected = !movedIndices
                .Select(x => expectedList[x] == inputList[x])
                .Any(x => x == false);

            Assert.IsTrue(movedAsExpected);
        }

        [DataTestMethod]
        [DynamicData(nameof(_dataSetsForRemoveOps))]
        public void GivenAnInputList_RemovingSelectedIndices_WillEndUpOnExpectedList(ArrangeableList<string> inputList, List<int> indicesToRemove, List<string> expectedList)
        {
            var removedIndices = inputList.RemoveAt(indicesToRemove);

            var movedAsExpected = !removedIndices
                .Select(x => expectedList[x] == inputList[x])
                .Any(x => x == false);

            Assert.IsTrue(movedAsExpected);
        }

        [TestMethod]
        public void WhenAnItemIsMovedUpTheListAppearsAsChanged()
        {
            var arrangeableList = new ArrangeableList<string>();
            arrangeableList.InitializeWith(new List<string> { "1", "2", "3" });

            arrangeableList.MoveItemsUpOnePosition(new List<int> { 1 });
            Assert.IsTrue(arrangeableList.ListHasChanged);
        }

        [TestMethod]
        public void WhenAnItemIsMovedDownTheListAppearsAsChanged()
        {
            var arrangeableList = new ArrangeableList<string>();
            arrangeableList.InitializeWith(new List<string> { "1", "2", "3" });

            arrangeableList.MoveItemsDownOnePosition(new List<int> { 1 });
            Assert.IsTrue(arrangeableList.ListHasChanged);
        }

        [TestMethod]
        public void WhenAnItemIsAddedTheListAppearsAsChanged()
        {
            var arrangeableList = new ArrangeableList<string>();
            arrangeableList.InitializeWith(new List<string> { "1", "2", "3" });

            arrangeableList.Add("new  element");
            Assert.IsTrue(arrangeableList.ListHasChanged);
        }

        [TestMethod]
        public void WhenAnItemIsRemovedTheListAppearsAsChanged()
        {
            var arrangeableList = new ArrangeableList<string>();
            arrangeableList.InitializeWith(new List<string> { "1", "2", "3" });

            arrangeableList.RemoveAt(1);
            Assert.IsTrue(arrangeableList.ListHasChanged);
        }

        [TestMethod]
        public void WhenAnItemChangesTheListAppearsAsChanged()
        {
            var arrangeableList = new ArrangeableList<string>();
            arrangeableList.InitializeWith(new List<string> { "1", "2", "3" });

            arrangeableList.ReplaceAt(2, "changed element");
            Assert.IsTrue(arrangeableList.ListHasChanged);
        }

        [TestMethod]
        public void WhenTheListIsInitializedAppearsAsNOTChanged()
        {
            var arrangeableList = new ArrangeableList<string>();
            arrangeableList.InitializeWith(new List<string> { "1", "2", "3" });

            Assert.IsFalse(arrangeableList.ListHasChanged);
        }
    }
}

