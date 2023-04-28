using HAMT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrie.Tests
{
    [TestClass]
    public partial class LeafTests
    {
        [TestMethod]
        public void Baseline()
        {
            // Arrange
            var hash = (uint)typeof(LeafTests).GetHashCode();
            var leaf = new HAMTrie<object>.Leaf(hash);

            // Validate
            Assert.AreEqual(hash, leaf.Hash);
            Assert.IsNull(leaf.Value);
        }


        [TestMethod]
        public void LevelOneConflict()
        {
            var data1 = "First";
            var data2 = "Second";
            var trie = new HAMTrie<string>();


            trie[0b_00000000_00000000_00000000_00000000] = data1;
            trie[0b_10000000_00000000_00000000_00000000] = data2;

            Assert.AreSame(data1, trie[0b_00000000_00000000_00000000_00000000]);
            Assert.AreSame(data2, trie[0b_10000000_00000000_00000000_00000000]);
        }

        [TestMethod]
        public void LeafConflict()
        {
            var data1 = "First";
            var data2 = "Second";
            var trie = new HAMTrie<string>();


            trie[0b_10000000_00000000_00000000_01000000] = data1;
            trie[0b_10000000_00000000_00000000_00000000] = data2;

            Assert.AreSame(data1, trie[0b_10000000_00000000_00000000_01000000]);
            Assert.AreSame(data2, trie[0b_10000000_00000000_00000000_00100000]);
        }

    }
}
