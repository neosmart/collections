using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoSmart.Collections.Tests
{
    [TestClass]
    public class UniqueSortedList
    {
        [TestMethod]
        public void TestUniqueCreation()
        {
            const int TotalCount = 200;
            var rng = new Random(42);
            List<int> notUnique = new List<int>(TotalCount);

            // Fill n slots with random numbers uniformly distributed from 0 to n/2
            for (int i = 0; i < TotalCount; ++i)
            {
                notUnique.Add(rng.Next(0, TotalCount >> 1));
            }

            var knownGood = new SortedSet<int>(notUnique);
            var unique = new UniqueSortedList<int>(notUnique);
            CollectionAssert.AreEqual(knownGood, unique);
        }
    }
}
