using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NeoSmart.Collections
{
    public class SortedList<T> : ICollection, ICollection<T>, IReadOnlyList<T>
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

        public int IndexOf(T item)
        {
            return _list.BinarySearch(item, _compare);
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
