using System;
using System.Collections.Generic;

namespace NeoSmart.Collections
{
    /// <summary>
    /// A wrapper around <c>List&lt;T&gt;</c> implementing <c>IDisposable</c>.
    /// When <see cref="Dispose()"/> is called, each item in the list is disposed.
    /// </summary>
    /// <typeparam name="T">The <see cref="IDisposable"/> type that will be stored in this disposable list.</typeparam>
    public class DisposableList<T> : List<T>, IDisposable
        where T : IDisposable
    {
        /// <summary>
        /// Initializes a new <see cref="DisposableList{T}"/>.
        /// </summary>
        public DisposableList() : base()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="DisposableList{T}"/> with the contents of the existing <c>IEnumerable&lt;T&gt;</c> <paramref name="enumerable"/>.
        /// </summary>
        /// <param name="list"></param>
        public DisposableList(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="DisposableList{T}"/> with the internal capacity set to <paramref name="capacity"/>.
        /// </summary>
        /// <param name="list"></param>
        public DisposableList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Disposes all of the <see cref="IDisposable"/> items in this list.
        /// </summary>
        /// <exception cref="AggregateException">Thrown if any of the individual calls to <see cref="T.Dispose"/> threw an exception.</exception>
        public void Dispose()
        {
            // Use a nullable type to avoid excess allocations in the happy case
            List<Exception>? exceptions = null;
            foreach (var item in this)
            {
                try
                {
                    item.Dispose();
                }
                catch (Exception ex)
                {
                    exceptions ??= new();
                    exceptions.Add(ex);
                }
            }

            if (exceptions?.Count > 0)
            {
                throw new AggregateException($"Exceptions encountered disposing {exceptions.Count} collection item(s)", exceptions);
            }

            base.Clear();
            base.Capacity = 0;
        }
    }
}
