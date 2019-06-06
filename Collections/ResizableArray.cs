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

        public bool IsSynchronized => ((ICollection)_inner).IsSynchronized;

        public object SyncRoot => ((ICollection)_inner).SyncRoot;

        public ResizableArray()
            : this(0)
        {
        }

        public ResizableArray(int initialSize)
        {
            _inner = new T[initialSize];
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

        public void Append(byte[] bytes)
        {
            Append(bytes, 0, bytes.Length);
        }

        public void Append(byte[] bytes, int startIndex, int count)
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
