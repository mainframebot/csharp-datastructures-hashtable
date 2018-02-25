using System;
using System.Collections;
using System.Collections.Generic;

namespace LinearProbingHashTableLesson
{
    public class LinearProbingHashTable<TKey, TValue> : IEnumerable<TKey>
        where TKey : IEquatable<TKey>
    {
        private int _pairCount;

        private int _hashSize;

        private TKey[] _keys;

        private TValue[] _values;

        public LinearProbingHashTable() : this(4)
        {
        }

        public LinearProbingHashTable(int capacity)
        {
            _hashSize = capacity;

            _keys = new TKey[capacity];
            _values = new TValue[capacity];
        }

        public bool Contains(TKey key)
        {
            try
            {
                Search(key);
                return true;
            }
            catch (KeyNotFoundException ex)
            {
                return false;
            }
        }

        public TValue Search(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException();

            for (var i = Hash(key); _keys[i] != null; i = (i + 1)%_hashSize)
            {
                if (_keys[i].Equals(key))
                    return _values[i];
            }

            throw new KeyNotFoundException();
        }

        public void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException();

            if(_pairCount >= _hashSize/2)
                Resize(2*_hashSize);

            int i;

            for (i = Hash(key); _keys[i] != null; i = (i + 1)%_hashSize)
            {
                if (_keys[i].Equals(key))
                {
                    _values[i] = value;
                    return;
                }
            }

            _keys[i] = key;
            _values[i] = value;

            _pairCount++;
        }

        public void Delete(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException();

            if (IsEmpty())
                throw new Exception();

            if (!Contains(key))
                return;

            var position = Hash(key);
            while (!key.Equals(_keys[position]))
            {
                position = (position + 1)%_hashSize;
            }

            position = (position + 1)%_hashSize;
            while (_keys[position] != null)
            {
                var reHashKey = _keys[position];
                var reHashValue = _values[position];

                _pairCount--;

                Insert(reHashKey, reHashValue);

                position = (position + 1)%_hashSize;
            }

            _pairCount--;

            if(_pairCount > 0 && _pairCount <= _hashSize/8)
                Resize(_hashSize*2);
        }

        public int Size()
        {
            return _pairCount;
        }

        public bool IsEmpty()
        {
            return Size() == 0;
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            for (var i = 0; i < _hashSize; i++)
            {
                yield return _keys[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Resize(int capacity)
        {
            var resizedHashTable = new LinearProbingHashTable<TKey, TValue>(capacity);

            for (var i = 0; i < _hashSize; i++)
            {
                if (_keys[i] != null)
                {
                    resizedHashTable.Insert(_keys[i], _values[i]);
                }
            }

            _keys = resizedHashTable._keys;
            _values = resizedHashTable._values;

            _hashSize = resizedHashTable._hashSize;
        }

        private int Hash(TKey key)
        {
            return (key.GetHashCode() & 0x7fffffff) % _hashSize;
        }
    }
}
