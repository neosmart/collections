using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoSmart.Collections.Tests
{
    internal class DisposeTester : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            if (Disposed)
            {
                throw new Exception("DisposeTester instance has already been disposed!");
            }

            Disposed = true;
        }
    }

    [TestClass]
    public class DisposableListTest
    {
        /// <summary>
        /// Ascertain that each item in a <see cref="DisposableList{T}"/> is disposed after calling
        /// <see cref="DisposableList{T}.Dispose"/>.
        /// </summary>
        [TestMethod]
        public void ValidateDispose()
        {
            var items = new List<DisposeTester> {
                new DisposeTester(),
                new DisposeTester(),
                new DisposeTester()
            };

            var disposableList = new DisposableList<DisposeTester>(items);
            disposableList.Dispose();

            Assert.IsTrue(items.All(item => item.Disposed));
        }

        /// <summary>
        /// Ascertain that exceptions during calls to <c>T.Dispose()</c> are returned as an
        /// <see cref="AggregateException"/>, and that all items were disposed even after an
        /// exception was encountered.
        /// <see cref="DisposableList{T}.Dispose"/>.
        /// </summary>
        [TestMethod]
        public void DisposeExceptionsBubbled()
        {
            var items = new List<DisposeTester> {
                new DisposeTester(),
                new DisposeTester(),
                new DisposeTester()
            };

            // Make it so the next call to DisposeTester.Dispose() will throw an exception.
            foreach (var item in items)
            {
                item.Dispose();
            }

            var disposableList = new DisposableList<DisposeTester>(items);
            // Assert that an AggregateException exception is thrown
            var thrown = Assert.ThrowsException<AggregateException>(disposableList.Dispose);
            Assert.IsInstanceOfType(thrown, typeof(AggregateException));
            // Assert that the exception contains an inner exception for each item
            Assert.IsTrue(thrown.InnerExceptions.Count == items.Count);
            // Assert that the items were all disposed despite the exceptions.
            Assert.IsTrue(items.All(item => item.Disposed));
        }

        [TestMethod]
        public void DisposableListInitializer()
        {
            // Assert that we can use initializer syntax with DisposableList<T>
            var disposableList = new DisposableList<DisposeTester> {
                new DisposeTester(),
                new DisposeTester(),
                new DisposeTester()
            };

            Assert.AreEqual(3, disposableList.Count);
        }

        /// <summary>
        /// Assert that the list is nulled after the call to <see cref="DisposableList{T}.Dispose"/>.
        /// </summary>
        [TestMethod]
        public void ListClearedAfterDispose()
        {
            // Assert that we can use initializer syntax with DisposableList<T>
            var disposableList = new DisposableList<DisposeTester> {
                new DisposeTester(),
                new DisposeTester(),
                new DisposeTester()
            };

            disposableList.Dispose();
            Assert.AreEqual(0, disposableList.Count);
            Assert.AreEqual(0, disposableList.Capacity);
        }
    }
}
