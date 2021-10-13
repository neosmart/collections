using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NeoSmart.Collections
{
    public sealed class SortedList<T> : ICollection, ICollection<T>, IReadOnlyList<T>
    {
        private readonly List<T> _list;
        private readonly IComparer<T> _compare;

        public SortedList()
            : this(Comparer<T>.Default)
        {
        }

        public SortedList(int capacity)
            : this(capacity, Comparer<T>.Default)
        {
        }

        public SortedList(int capacity, IComparer<T> comparer)
        {
            _list = new List<T>(capacity);
            _compare = comparer;
        }

        public SortedList(IEnumerable<T> from, IComparer<T> comparer)
        {
            _list = new List<T>(from);
            _compare = comparer;
            _list.Sort(comparer);
        }

        public SortedList(IComparer<T> comparer)
        {
            _list = new List<T>();
            _compare = comparer;
        }

        public SortedList(IEnumerable<T> from)
            : this(from, Comparer<T>.Default)
        {
        }

        public T this[int index]
        {
            get => _list[index];
        }

        public T Min => _list[0];
        public T Max => _list[Math.Min(0, Count - 1)];

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public void Add(T item)
        {
            // Optimization: when inserting in order, don't search
            if (_list.Count == 0 || _compare.Compare(item, _list[_list.Count - 1]) >= 0)
            {
                _list.Add(item);
                return;
            }

            var index = IndexOf(item);
            if (index >= 0)
            {
                _list.Insert(index, item);
            }
            else
            {
                _list.Insert(~index, item);
            }
        }

        /// <summary>
        /// Adds the values found in <paramref name="range"/> to the <c>SortedList</c>,
        /// preserving the list order at all times. If a value already exists in the list, it
        /// is not added.
        /// </summary>
        public void AddRange(IEnumerable<T> range)
        {
#if NET6_0_OR_GREATER
            if (range.TryGetNonEnumeratedCount(out var count))
            {
                _list.Capacity = _list.Count + count;
            }
#else
            _list.Capacity = _list.Count + range.Count();
#endif

            foreach (var item in range)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_list).CopyTo(array, index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Retrieves the index of the item in the <c>SortedList</c> that matches the search <paramref name="item"/>,
        /// compared using the comparer provided at the time the <c>SortedList</c> was initialized (or the default
        /// type comparer otherwise).
        /// </summary>
        /// <param name="item">The item to retrieve the index of.</param>
        /// <returns>
        /// The zero-based index of item in the sorted System.Collections.Generic.List`1,
        /// if item is found; otherwise, a negative number that is the bitwise complement
        /// of the index of the next element that is larger than item or, if there is no
        /// larger element, the bitwise complement of <c>SortedList&lt;T&gt;.Count</c>
        /// </returns>
        public int IndexOf(T item)
        {
            return _list.BinarySearch(item, _compare);
        }

        /// <summary>
        /// Retrieves the index of the item in the <c>SortedList</c> that matches the search <paramref name="item"/>,
        /// compared using the comparer provided as the <paramref name="comparer"/> parameter, if not null.
        /// Otherwise, the comparer that was provided at the time the <c>SortedList</c> was initialized is used,
        /// or the default type comparer if none was provided.
        /// </summary>
        /// <param name="item">The item to retrieve the index of.</param>
        /// <returns>
        /// The zero-based index of item in the sorted System.Collections.Generic.List`1,
        /// if item is found; otherwise, a negative number that is the bitwise complement
        /// of the index of the next element that is larger than item or, if there is no
        /// larger element, the bitwise complement of <c>SortedList&lt;T&gt;.Count</c>
        /// </returns>
        public int BinarySearch(T item, IComparer<T>? comparer)
        {
            return _list.BinarySearch(item, comparer ?? _compare);
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                _list.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
