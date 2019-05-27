using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SortedList.Tests
{
    [TestClass]
    public class SortedListTests
    {
        [TestMethod]
        public void ImplementsTypes()
        {
            var x = new SortedList<int>();

            Assert.IsInstanceOfType(x, typeof(ICollection));
            Assert.IsInstanceOfType(x, typeof(ICollection<int>));
            Assert.IsInstanceOfType(x, typeof(IReadOnlyList<int>));
            Assert.IsInstanceOfType(x, typeof(IReadOnlyCollection<int>));
        }

        private List<int> GetUnsortedNumbers(int count)
        {
            var list = new List<int>(count);
            var rng = new System.Random(0xC0FFEE ^ count);

            for (int i = 0; i < count; ++i)
            {
                list.Add(rng.Next());
            }

            return list;
        }

        [TestMethod]
        public void BasicSort()
        {
            var unsorted = GetUnsortedNumbers(4200);
            var sorted = new SortedSet<int>(unsorted);
            var ourSorted = new SortedList<int>(unsorted);

            Assert.AreEqual(sorted.Count, ourSorted.Count);
            CollectionAssert.AreEqual(sorted, ourSorted);
        }

        [TestMethod]
        public void EmptyListTests()
        {
            var sorted = new SortedList<int>();
            Assert.IsFalse(sorted.Contains(12));
            Assert.AreEqual(-1, sorted.IndexOf(17));
        }

        [TestMethod]
        public void SingleEntryTests()
        {
            var sorted = new SortedList<int>();
            sorted.Add(4);
            Assert.AreEqual(0, sorted.IndexOf(4));
            Assert.IsTrue(sorted.IndexOf(3) < 0);
            Assert.IsTrue(sorted.IndexOf(5) < 0);

            Assert.IsTrue(~sorted.IndexOf(3) == 0);
        }

        [TestMethod]
        public void FasterForAlreadySorted()
        {
            var sortedset = new SortedSet<int>();
            var sortedlist = new SortedList<int>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 10_000; ++i)
            {
                sortedset.Add(i);
            }
            stopwatch.Stop();

            var time1 = stopwatch.ElapsedTicks;

            stopwatch.Restart();
            for (int i = 0; i < 10_000; ++i)
            {
                sortedlist.Add(i);
            }
            stopwatch.Stop();

            var time2 = stopwatch.ElapsedTicks;

            Assert.IsTrue(time2 < time1);
        }

        [TestMethod]
        public void FasterForUnsorted()
        {
            var unsorted = GetUnsortedNumbers(10_000);
            var stopwatch = new Stopwatch();

            var sortedset = new SortedSet<int>();
            stopwatch.Start();
            for (int i = 0; i < unsorted.Count; ++i)
            {
                sortedset.Add(i);
            }
            stopwatch.Stop();

            var time1 = stopwatch.ElapsedTicks;

            var sortedlist = new SortedList<int>();
            stopwatch.Restart();
            for (int i = 0; i < unsorted.Count; ++i)
            {
                sortedlist.Add(i);
            }
            stopwatch.Stop();

            var time2 = stopwatch.ElapsedTicks;

            Assert.IsTrue(time2 < time1);
        }
    }
}
