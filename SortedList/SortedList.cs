using System;
using System.Collections;
using System.Collections.Generic;

namespace SortedList
{
    public class SortedList<T> : ICollection, ICollection<T>, IReadOnlyList<T>
        where T: IComparable<T>
    {
        private List<T> _list;

        public SortedList()
        {
            _list = new List<T>();
        }

        public SortedList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        public SortedList(IEnumerable<T> from)
        {
            _list = new List<T>(from);
            _list.Sort();
        }

        public T this[int index]
        {
            get => _list[index];
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        public void Add(T item)
        {
            // Optimization: when inserting in order, don't search
            if (_list.Count == 0 || item.CompareTo(_list[_list.Count - 1]) >= 0)
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

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) > 0;
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
            return _list.BinarySearch(item);
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
