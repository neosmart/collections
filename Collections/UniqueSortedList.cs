using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NeoSmart.Collections
{
    public sealed class UniqueSortedList<T> : ICollection, ICollection<T>, IReadOnlyList<T>
    {
        private readonly List<T> _list;
        private readonly IComparer<T> _compare;

        public UniqueSortedList()
            : this(Comparer<T>.Default)
        {
        }

        public UniqueSortedList(int capacity)
            : this(capacity, Comparer<T>.Default)
        {
        }

        public UniqueSortedList(int capacity, IComparer<T> comparer)
        {
            _list = new List<T>(capacity);
            _compare = comparer;
        }

        public UniqueSortedList(IEnumerable<T> from, IComparer<T> comparer)
        {
            _list = new List<T>(from);
            _compare = comparer;
            _list.Sort(comparer);

            // Now deduplicate the list. To avoid memory thrashing, swap duplicates
            // with the end of the list, which is faster but requires another sort
            // once we're done.
            int newEnd = _list.Count - 1;
            for (int i = _list.Count - 1; i > 0; --i)
            {
                if (comparer.Compare(_list[i], _list[i - 1]) == 0)
                {
                    // Duplicate found, move it to the end of the list
                    _list[i] = _list[newEnd];
                    --newEnd;
                }
            }

            if (newEnd != _list.Count - 1)
            {
                // One or more duplicates were found; trim list at new end
                _list.RemoveRange(newEnd + 1, _list.Count - newEnd - 1);

                // And sort the list one final time
                _list.Sort(_compare);
            }
        }

        public UniqueSortedList(IComparer<T> comparer)
        {
            _list = new List<T>();
            _compare = comparer;
        }

        public UniqueSortedList(IEnumerable<T> from)
            : this(from, Comparer<T>.Default)
        {
        }

        public T this[int index] => _list[index];

        public T Min => _list[0];
        public T Max => _list[Math.Min(0, Count - 1)];

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        void ICollection<T>.Add(T item) => Add(item);

        public bool Add(T item)
        {
            // Optimization: when inserting in order, don't search
            //if (_list.Count == 0 || _compare.Compare(item, _list[_list.Count - 1]) >= 0)
            if (_list.Count == 0 || _compare.Compare(item, _list[_list.Count - 1]) > 0)
            {
                _list.Add(item);
                return true;
            }

            var index = IndexOf(item);
            if (index >= 0)
            {
                // Item already in set
                return false;
            }
            else
            {
                _list.Insert(~index, item);
                return true;
            }
        }

        /// <summary>
        /// Adds the values found in <paramref name="range"/> to the <c>UniqueSortedList</c>,
        /// preserving the list order at all times. If a value already exists in the list, it
        /// is not added.
        /// </summary>
        /// <param name="range"></param>
        /// <returns>The total number of items added, duplicates excluded.</returns>
        public int AddRange(IEnumerable<T> range)
        {
#if NET6_0_OR_GREATER
            if (range.TryGetNonEnumeratedCount(out var count))
            {
                _list.Capacity = _list.Count + count;
            }
#else
            _list.Capacity = _list.Count + range.Count();
#endif

            int added = 0;
            foreach (var item in range)
            {
                added += Add(item) ? 1 : 0;
            }

            return added;
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
        /// Retrieves the index of the item in the <c>UniqueSortedList</c> that matches the search <paramref name="item"/>,
        /// compared using the comparer provided at the time the <c>UniqueSortedList</c> was initialized (or the default
        /// type comparer otherwise).
        /// </summary>
        /// <param name="item">The item to retrieve the index of.</param>
        /// <returns>
        /// The zero-based index of item in the sorted System.Collections.Generic.List`1,
        /// if item is found; otherwise, a negative number that is the bitwise complement
        /// of the index of the next element that is larger than item or, if there is no
        /// larger element, the bitwise complement of <c>UniqueSortedList&lt;T&gt;.Count</c>
        /// </returns>
        public int IndexOf(T item)
        {
            return _list.BinarySearch(item, _compare);
        }

        /// <summary>
        /// Retrieves the index of the item in the <c>UniqueSortedList</c> that matches the search <paramref name="item"/>,
        /// compared using the comparer provided as the <paramref name="comparer"/> parameter, if not null.
        /// Otherwise, the comparer that was provided at the time the <c>UniqueSortedList</c> was initialized is used,
        /// or the default type comparer if none was provided.
        /// </summary>
        /// <param name="item">The item to retrieve the index of.</param>
        /// <returns>
        /// The zero-based index of item in the sorted System.Collections.Generic.List`1,
        /// if item is found; otherwise, a negative number that is the bitwise complement
        /// of the index of the next element that is larger than item or, if there is no
        /// larger element, the bitwise complement of <c>UniqueSortedList&lt;T&gt;.Count</c>
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
