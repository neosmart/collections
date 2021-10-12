using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NeoSmart.Collections
{
    public class ResizableArray<T> :  ICollection, IReadOnlyList<T>
    {
        private T[] _array;
        public T[] Array => _array;

        public int Count => _array.Length;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public ResizableArray()
            : this(0)
        {
        }

        public ResizableArray(int initialSize)
        {
            _array = new T[initialSize];
        }

        public ResizableArray(IReadOnlyList<T> values)
        {
            // _array is initialized by CopyReadOnlyList()
            _array = System.Array.Empty<T>();
            CopyReadOnlyList(values);
        }

        public ResizableArray(IEnumerable<T> values)
        {
            if (values is IReadOnlyList<T> list)
            {
                // _array is initialized by CopyReadOnlyList()
                _array = System.Array.Empty<T>();
                CopyReadOnlyList(list);
                return;
            }

            int initialCapacity = 2;
            _array = new T[initialCapacity];

            int i = 0;
            foreach (var t in values)
            {
                if (i == _array.Length)
                {
                    Resize(_array.Length * 2);
                }

                _array[i++] = t;
            }

            // As we pre-emptively resize to avoid thrashing the heap, we now need to resize down
            Resize(i);
        }

        private void CopyReadOnlyList(IReadOnlyList<T> values)
        {
            _array = new T[values.Count];
            for (int i = 0; i < values.Count; ++i)
            {
                _array[i] = values[i];
            }
        }

        public void Resize(int size)
        {
            System.Array.Resize(ref _array, size);
        }

        public T this[int index]
        {
            get => _array[index];
            set => _array[index] = value;
        }

        public void Append(T[] values)
        {
            Append(values, 0, values.Length);
        }

        public void Append(T[] values, int startIndex, int count)
        {
            int oldIndex = Array.Length;
            System.Array.Resize(ref _array, Array.Length + count);
            System.Array.Copy(values, startIndex, _array, oldIndex, count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)_array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _array.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            _array.CopyTo(array, index);
        }
    }
}
