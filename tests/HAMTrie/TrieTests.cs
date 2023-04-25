using HAMT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrie.Tests
{
    [TestClass]
    public partial class HAMTrieTests
    {
        [TestMethod]
        public void Int()
        {
            var trie = new HAMTrie<uint>();

            var instance = typeof(HAMTrieTests);
            var hash = (uint)instance.GetHashCode();

            trie[hash] = hash;

            Assert.AreEqual(hash, trie[hash]);
        }

        [TestMethod]
        public void Object()
        {
            var data = new object();
            var trie = new HAMTrie<object>();

            var hash = (uint)data.GetHashCode();

            trie[hash] = data;

            Assert.AreSame(data, trie[hash]);
        }

        [TestMethod]
        public void LevelConflict()
        {
            var data1 = "First";
            var data2 = "Second";
            var trie = new HAMTrie<string>();

            var hash1 = (uint)data1.GetHashCode();
            var hash2 = (uint)hash1 + 1;

            trie[hash1] = data1;
            trie[hash2] = data2;

            Assert.AreSame(data1, trie[hash1]);
            Assert.AreSame(data2, trie[hash2]);
        }

    }
}
