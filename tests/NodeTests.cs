using HAMT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Node.Tests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void DefaultCtor()
        {
            // Act
            var node = new TrieNode();

            // Validate
            Assert.AreEqual((ulong)0, node.Bitmap);
            Assert.IsNotNull(node.Nodes);
            Assert.AreEqual(0, node.Nodes.Length);
        }

        [TestMethod]
        public void LevelCtor()
        {
            // Arrange
            ulong flags = ulong.MaxValue / 2;
            var nodes = new INode[0];

            // Act
            var node = new TrieNode(flags, nodes);

            // Validate
            Assert.AreEqual(flags, node.Bitmap);
            Assert.IsNotNull(node.Nodes);
            Assert.AreSame(nodes, node.Nodes);
        }

        [TestMethod]
        public void SplitCtor()
        {
            // Arrange
            ulong flags = ulong.MaxValue / 2;
            var nodes = new INode[0];

            // Act
            var node = new TrieNode(flags, nodes);

            // Validate
            Assert.AreEqual(flags, node.Bitmap);
            Assert.IsNotNull(node.Nodes);
            Assert.AreSame(nodes, node.Nodes);
        }


        [TestMethod]
        public void Split()
        {
            var id = System.Environment.CurrentManagedThreadId;
            var multiplier = id * 4 + 5;
            var m_dwHashCodeSeed = id;
            
            var node1 = new object().GetHashCode();

            while (m_dwHashCodeSeed < node1)
                m_dwHashCodeSeed = m_dwHashCodeSeed * multiplier + 1;

            // Arrange
            var node2 = new object().GetHashCode();
            var node3 = (node2 - 1) / multiplier;;
            var node4 = new object().GetHashCode();

            // Validate
            Assert.IsTrue(!node1.Equals(node2));
        }
    }
}
