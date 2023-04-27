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
    }
}
