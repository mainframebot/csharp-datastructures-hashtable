using NUnit.Framework;
using SeparateChainingHashTableLesson;

namespace SeparateChainingHashTableTests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Testing()
        {
            var hashTable = new SeparateChainingHashTable<string, int>();

            hashTable.Insert("S", 0);
            hashTable.Insert("E", 1);
            hashTable.Insert("A", 2);
            hashTable.Insert("R", 3);
            hashTable.Insert("C", 4);
            hashTable.Insert("H", 5);
            hashTable.Insert("E", 6);
            hashTable.Insert("X", 7);
            hashTable.Insert("A", 8);
            hashTable.Insert("M", 9);
            hashTable.Insert("P", 10);
            hashTable.Insert("L", 11);
            hashTable.Insert("E", 12);

            var result = hashTable.Search("X");
        }
    }
}
