using System;
using System.Collections;
using System.Collections.Generic;

namespace SeparateChainingHashTableLesson
{
    public class SeparateChainingHashTable<TKey, TValue> : IEnumerable<TKey>
        where TKey : IEquatable<TKey>
    {
        private int _pairCount;

        private int _hashSize;

        private IDictionary<TKey, TValue>[] _dictionary;

        public SeparateChainingHashTable() : this(4)
        {
        }

        public SeparateChainingHashTable(int capacity)
        {
            _hashSize = capacity;

            _dictionary = new IDictionary<TKey, TValue>[capacity];

            for(var i = 0; i < capacity; i++)
                _dictionary[i] = new Dictionary<TKey, TValue>();
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
            if(key == null)
                throw new ArgumentNullException();

            var position = Hash(key);

            TValue result;

            if(!_dictionary[position].TryGetValue(key, out result))
                throw new KeyNotFoundException();

            return result;
        }

        public void Insert(TKey key, TValue value)
        {
            if(key == null)
                throw new ArgumentNullException();

            if(_pairCount >= 10*_hashSize)
                Resize(2*_hashSize);

            var position = Hash(key);

            if (_dictionary[position].ContainsKey(key))
                return;

            _dictionary[position].Add(key, value);
            _pairCount++;
        }

        public void Delete(TKey key)
        {
            if(key == null)
                throw new ArgumentNullException();

            if(IsEmpty())
                throw new Exception();

            if (!Contains(key))
                return;

            var position = Hash(key);

            if (_dictionary[position].ContainsKey(key))
                _pairCount--;

            _dictionary[position].Remove(key);

            if(_hashSize > 4 && _pairCount < 2*_hashSize)
                Resize(_hashSize/2);
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
                foreach (var entry in _dictionary[i])
                {
                    yield return entry.Key;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Resize(int capacity)
        {
            var resizedHashTable = new SeparateChainingHashTable<TKey, TValue>(capacity);

            for (var i = 0; i < _hashSize; i++)
            {
                foreach (var entry in _dictionary[i])
                {
                    resizedHashTable.Insert(entry.Key, entry.Value);
                }
            }

            _pairCount = resizedHashTable._pairCount;
            _hashSize = resizedHashTable._hashSize;

            _dictionary = resizedHashTable._dictionary;
        }

        private int Hash(TKey key)
        {
            return (key.GetHashCode() & 0x7fffffff)% _hashSize;
        }
    }
}
