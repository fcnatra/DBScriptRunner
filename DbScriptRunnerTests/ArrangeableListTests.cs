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

        [TestMethod]
        public void GivenAPersistedDatabaseList_WhenLoadingListBack_ListIsTheSame()
        {
            // GIVEN
            var databases = new List<DbScriptRunnerLogic.Entities.Database>()
            {
                new DbScriptRunnerLogic.Entities.Database{Name = "one"},
                new DbScriptRunnerLogic.Entities.Database{Name = "two"}
            };
            var expectedNumberOfDatabases = databases.Count();
            string repositoryContent = "";

            var fakeRepository = A.Fake<DbScriptRunnerLogic.Interfaces.IRepository>();
            fakeRepository.Location = "c:\\temp";
            fakeRepository.Name = "testing.txt";

            var dataPersistence = new DbScriptRunnerLogic.DataPersistence
            {
                Repository = fakeRepository
            };
            var app = new DbScriptRunner.Services.AppData();

            app.Databases = new System.Collections.Generic.List<DbScriptRunnerLogic.Interfaces.INamed>();
            ((List<INamed>)app.Databases).AddRange(databases);

            A.CallTo(() => fakeRepository.Save(A<string>.That.IsNotNull())).Invokes((x) => repositoryContent = (string)x.Arguments.First());

            app.DataPersistence = dataPersistence;
            app.SaveDatabases(fakeRepository.Name, fakeRepository.Location);
            app.BackupCurrentStatus();

            // WHEN
            A.CallTo(() => fakeRepository.Load()).Returns(repositoryContent);

            var secondInstanceSameUser = new DbScriptRunner.Services.AppData();
            secondInstanceSameUser.DataPersistence = dataPersistence;
            secondInstanceSameUser.RecoverLastStatus();

            Assert.AreEqual(expectedNumberOfDatabases, app.Databases.Count());
            var expectedDatabasesWereFound = true;
            foreach (var db in app.Databases)
            {
                expectedDatabasesWereFound = databases.Any(expectedDb => expectedDb.Name == db.Name);
                if (!expectedDatabasesWereFound) break;
            }

            // THEN
            Assert.IsTrue(expectedDatabasesWereFound);
        }

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
            var removedIndices = inputList.RemoveItems(indicesToRemove);

            var movedAsExpected = !removedIndices
                .Select(x => expectedList[x] == inputList[x])
                .Any(x => x == false);

            Assert.IsTrue(movedAsExpected);
        }
    }
}

