using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoSmart.Collections.Tests
{
    [TestClass]
    public class ResizableArrayTests
    {
        [TestMethod]
        public void InitTests()
        {
            var array = new ResizableArray<int>();
            Assert.AreEqual(0, array.Count);
            array = new ResizableArray<int>(20);
            Assert.AreEqual(20, array.Count);
        }

        [TestMethod]
        public void InitFromExisting()
        {
            // From IReadOnlyList<T>
            var existing = new[] { 1, 2, 3 };
            var array = new ResizableArray<int>(existing);
            CollectionAssert.AreEqual(existing, array);

            // From non-indexable IEnumerable<T>
            var existingSet = new SortedSet<int> { 1, 2, 3 };
            array = new ResizableArray<int>(existingSet);
            CollectionAssert.AreEqual(existingSet, array);
        }

        [TestMethod]
        public void ResizeTests()
        {
            var array = new ResizableArray<int>(12);
            array.Resize(24);
            Assert.AreEqual(24, array.Count);
        }

        [TestMethod]
        public void Shrink()
        {
            var array = new ResizableArray<int>(8);
            array.Resize(8);
            Assert.AreEqual(8, array.Count);
        }

        [TestMethod]
        public void ArrayAppend()
        {
            var values1 = new[] { 1, 2, 3, 4 };
            var values2 = new[] { 5, 6 };

            var array = new ResizableArray<int>(values1);
            array.Append(values2);
            var combined = values1.ToList();
            combined.AddRange(values2);

            CollectionAssert.AreEqual(combined, array);
        }
    }
}
