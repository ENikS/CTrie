using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

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
            var leaf = new CTrieSet<object>.Leaf(hash);

            // Validate
            Assert.AreEqual(hash, leaf.Hash);
            Assert.IsNull(leaf.Value);
        }

        [TestMethod]
        public void Rotate()
        {
            // Arrange
            var hash = (uint)typeof(LeafTests).GetHashCode();
            var leaf = new CTrieSet<object>.Leaf(hash);

            // Act
            hash = BitOperations.RotateLeft(hash, 6);
            leaf.Rotate();

            // Validate
            Assert.AreEqual(hash, leaf.Hash);
        }

    }
}
