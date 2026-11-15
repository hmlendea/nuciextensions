using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class ListExtensionsTests
    {
        [Test]
        public void GivenAPopulatedList_WhenShuffling_ThenTheSameElementsAreReturned()
        {
            IList<int> collection = [4, 8, 16, 32, 42, 48, 64, 96];

            IList<int> shuffledCollection = collection.Shuffle();

            Assert.That(shuffledCollection, Is.EquivalentTo(collection));
            Assert.That(shuffledCollection, Is.Not.SameAs(collection));
        }

        [Test]
        [TestCase(new[] { 1, 2, 3, 4, 5 })]
        [TestCase(new[] { 10, 20, 30 })]
        [TestCase(new[] { 100 })]
        [TestCase(new[] { -1, -2, -3, -4 })]
        [TestCase(new[] { 1, 1, 2, 2, 3, 3 })]
        public void GivenAPopulatedList_WhenShuffling_ThenTheSameElementsAreReturned_Parameterized(int[] collection)
        {
            var list = new List<int>(collection);
            var shuffled = list.Shuffle();
            Assert.That(shuffled, Is.EquivalentTo(list));
            Assert.That(shuffled, Is.Not.SameAs(list));
        }

        [Test]
        public void GivenAPopulatedList_WhenShuffling_ThenTheCountIsPreserved()
        {
            IList<int> collection = [4, 8, 16, 32, 42, 48, 64, 96];

            IList<int> shuffledCollection = collection.Shuffle();

            Assert.That(shuffledCollection, Has.Count.EqualTo(collection.Count));
        }

        [Test]
        [TestCase(new[] { 1, 2, 3, 4, 5 })]
        [TestCase(new[] { 10, 20, 30 })]
        [TestCase(new[] { 100 })]
        [TestCase(new[] { -1, -2, -3, -4 })]
        [TestCase(new[] { 1, 1, 2, 2, 3, 3 })]
        public void GivenAPopulatedList_WhenShuffling_ThenTheCountIsPreserved_Parameterized(int[] collection)
        {
            var list = new List<int>(collection);
            var shuffled = list.Shuffle();
            Assert.That(shuffled, Has.Count.EqualTo(list.Count));
        }

        [Test]
        public void GivenANullList_WhenShuffling_ThenAnArgumentNullExceptionIsThrown()
        {
            IList<int> collection = null;

            Assert.That(
                () => collection.Shuffle(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenAnEmptyList_WhenShuffling_ThenTheOriginalListIsReturned()
        {
            IList<int> collection = [];

            Assert.That(collection.Shuffle(), Is.SameAs(collection));
        }

        [Test]
        public void GivenASingleElementList_WhenShuffling_ThenTheElementIsReturned()
        {
            IList<int> collection = [42];

            Assert.That(collection.Shuffle(), Is.EqualTo(collection));
        }

        [Test]
        public void GivenAPopulatedList_WhenPopping_ThenTheLastElementIsRemoved()
        {
            IList<int> collection = [4, 8, 16, 42];
            IEnumerable<int> expectedCollection = [4, 8, 16];

            collection.Pop();

            Assert.That(collection, Is.EqualTo(expectedCollection));
        }

        [Test]
        [TestCase(new[] { 1, 2, 3 }, 3, new[] { 1, 2 })]
        [TestCase(new[] { 10, 20 }, 20, new[] { 10 })]
        [TestCase(new[] { 100 }, 100, new int[] { })]
        [TestCase(new[] { -1, -2, -3 }, -3, new[] { -1, -2 })]
        public void GivenAPopulatedList_WhenPopping_ThenTheLastElementIsRemoved_Parameterized(
            int[] initial, int expectedPopped, int[] expectedRemaining)
        {
            var list = new List<int>(initial);
            int popped = list.Pop();
            Assert.That(popped, Is.EqualTo(expectedPopped));
            Assert.That(list, Is.EqualTo(expectedRemaining));
        }

        [Test]
        public void GivenAPopulatedList_WhenPopping_ThenTheLastElementIsReturned()
        {
            IList<int> collection = [4, 8, 16, 42];

            int poppedElement = collection.Pop();

            Assert.That(poppedElement, Is.EqualTo(42));
        }

        [Test]
        [TestCase(new[] { 1, 2, 3 }, 3)]
        [TestCase(new[] { 10, 20 }, 20)]
        [TestCase(new[] { 100 }, 100)]
        [TestCase(new[] { -1, -2, -3 }, -3)]
        public void GivenAPopulatedList_WhenPopping_ThenTheLastElementIsReturned_Parameterized(
            int[] initial, int expectedPopped)
        {
            var list = new List<int>(initial);
            int popped = list.Pop();
            Assert.That(popped, Is.EqualTo(expectedPopped));
        }

        [Test]
        public void GivenAnEmptyList_WhenPopping_ThenAnInvalidOperationExceptionIsThrown()
        {
            IList<int> collection = [];

            Assert.That(
                () => collection.Pop(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GivenANullList_WhenPopping_ThenAnArgumentNullExceptionIsThrown()
        {
            IList<int> collection = null;

            Assert.That(
                () => collection.Pop(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenMultipleThreads_WhenShuffling_ThenNoFailureOccursAndEveryElementIsPreserved()
        {
            const int TaskCount = 8;
            const int IterationsPerTask = 2_000;

            IList<int> source = [4, 8, 16, 32, 42, 48, 64, 96];
            int[] expected = [.. source];
            ConcurrentQueue<Exception> failures = [];
            ConcurrentQueue<int[]> results = [];

            Task[] tasks = new Task[TaskCount];

            for (int taskIndex = 0; taskIndex < TaskCount; taskIndex++)
            {
                tasks[taskIndex] = Task.Run(() =>
                {
                    try
                    {
                        for (int iteration = 0; iteration < IterationsPerTask; iteration++)
                        {
                            results.Enqueue([.. source.Shuffle()]);
                        }
                    }
                    catch (Exception exception)
                    {
                        failures.Enqueue(exception);
                    }
                });
            }

            Task.WaitAll(tasks);

            Assert.That(failures, Is.Empty, string.Join(Environment.NewLine, failures.Select(failure => failure.ToString())));
            Assert.That(results, Has.Count.EqualTo(TaskCount * IterationsPerTask));
            Assert.That(results, Is.All.EquivalentTo(expected));
        }

        [Test]
        public void GivenMultipleThreads_WhenShuffling_ThenTheSourceListIsNeverModified()
        {
            IList<int> source = [4, 8, 16, 32, 42, 48, 64, 96];
            int[] expected = [.. source];
            ConcurrentQueue<Exception> failures = [];

            Task[] tasks = new Task[8];

            for (int taskIndex = 0; taskIndex < tasks.Length; taskIndex++)
            {
                tasks[taskIndex] = Task.Run(() =>
                {
                    try
                    {
                        for (int iteration = 0; iteration < 2_000; iteration++)
                        {
                            source.Shuffle();
                        }
                    }
                    catch (Exception exception)
                    {
                        failures.Enqueue(exception);
                    }
                });
            }

            Task.WaitAll(tasks);

            Assert.That(failures, Is.Empty, string.Join(Environment.NewLine, failures.Select(failure => failure.ToString())));
            Assert.That(source, Is.EqualTo(expected));
        }
    }
}
