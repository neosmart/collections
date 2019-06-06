using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NeoSmart.Collections
{
    public class ResizableArray<T> :  ICollection, IReadOnlyList<T>
    {
        private T[] _inner;
        public T[] Inner => _inner;

        public int Count => _inner.Length;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public ResizableArray()
            : this(0)
        {
        }

        public ResizableArray(int initialSize)
        {
            _inner = new T[initialSize];
        }

        public ResizableArray(IReadOnlyList<T> values)
        {
            CopyReadOnlyList(values);
        }

        public ResizableArray(IEnumerable<T> values)
        {
            if (values is IReadOnlyList<T> list)
            {
                CopyReadOnlyList(list);
                return;
            }

            int initialCapacity = 2;
            _inner = new T[initialCapacity];

            int i = 0;
            foreach (var t in values)
            {
                if (i == _inner.Length)
                {
                    Resize(_inner.Length * 2);
                }

                _inner[i++] = t;
            }

            // As we pre-emptively resize to avoid thrashing the heap, we now need to resize down
            Resize(i);
        }

        private void CopyReadOnlyList(IReadOnlyList<T> values)
        {
            _inner = new T[values.Count];
            for (int i = 0; i < values.Count; ++i)
            {
                _inner[i] = values[i];
            }
        }

        public void Resize(int size)
        {
            Array.Resize(ref _inner, size);
        }

        public T this[int index]
        {
            get => _inner[index];
            set => _inner[index] = value;
        }

        public void Append(T[] bytes)
        {
            Append(bytes, 0, bytes.Length);
        }

        public void Append(T[] bytes, int startIndex, int count)
        {
            int oldIndex = Inner.Length;
            Array.Resize(ref _inner, Inner.Length + count);
            Array.Copy(bytes, startIndex, _inner, oldIndex, count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)_inner).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            _inner.CopyTo(array, index);
        }
    }
}
