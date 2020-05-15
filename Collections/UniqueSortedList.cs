using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NeoSmart.Collections
{
    public class UniqueSortedList<T> : ICollection, ICollection<T>, IReadOnlyList<T>
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
        public T Max => _list[Count - 1];

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public object? SyncRoot => null;

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
