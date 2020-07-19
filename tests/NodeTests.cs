using CTrie;
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
            var node = new CTrie.Node();

            // Validate
            Assert.AreEqual((ulong)0, node.Flags);
            Assert.AreEqual((ulong)0, node.Leafs);
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
            var node = new CTrie.Node(flags, nodes);

            // Validate
            Assert.AreEqual(flags, node.Flags);
            Assert.AreEqual((ulong)0, node.Leafs);
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
            var node = new CTrie.Node(flags, flags, nodes);

            // Validate
            Assert.AreEqual(flags, node.Flags);
            Assert.AreEqual(flags, node.Leafs);
            Assert.IsNotNull(node.Nodes);
            Assert.AreSame(nodes, node.Nodes);
        }
    }
}
