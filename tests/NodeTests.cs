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

        [TestMethod]
        public void LeafCtorEmpty()
        {
            // Arrange
            var instance = new object();
            uint hash = (uint)instance.GetHashCode();
            var leaf = new CTrieSet<object>.Leaf(hash);

            ulong position = 4;

            // Act
            var node = new CTrie.Node(position, leaf, new CTrie.Node());

            // Validate
            Assert.AreEqual(position, node.Flags);
            Assert.AreEqual(position, node.Leafs);
            Assert.IsNotNull(node.Nodes);
            Assert.AreEqual(1, node.Nodes.Length);
        }

        [TestMethod]
        public void LeafCtorOne()
        {
            // Arrange
            ulong flag = 1;
            var instance = new object();
            uint hash = (uint)instance.GetHashCode();
            var leaf = new CTrieSet<object>.Leaf(hash);
            var empty = new CTrie.Node();
            var parent = new CTrie.Node(flag, new INode[] { empty });

            ulong position = 4;

            // Act
            var node = new CTrie.Node(position, leaf, parent);

            // Validate
            Assert.AreEqual(position | flag, node.Flags);
            Assert.AreEqual(position, node.Leafs);
            Assert.IsNotNull(node.Nodes);
            Assert.AreEqual(2, node.Nodes.Length);

            Assert.AreSame(empty, node.Nodes[0]);
            Assert.AreSame(leaf,  node.Nodes[1]);
        }

        [TestMethod]
        public void LeafCtorTwo()
        {
            // Arrange
            ulong flag = 9;
            var instance = new object();
            uint hash = (uint)instance.GetHashCode();
            var leaf = new CTrieSet<object>.Leaf(hash);
            var empty1 = new CTrie.Node();
            var empty2 = new CTrie.Node();
            var parent = new CTrie.Node(flag, new INode[] { empty1, empty2 });

            ulong position = 4;

            // Act
            var node = new CTrie.Node(position, leaf, parent);

            // Validate
            Assert.AreEqual(position | flag, node.Flags);
            Assert.AreEqual(position, node.Leafs);
            Assert.IsNotNull(node.Nodes);
            Assert.AreEqual(3, node.Nodes.Length);

            Assert.AreSame(empty1, node.Nodes[0]);
            Assert.AreSame(leaf,   node.Nodes[1]);
            Assert.AreSame(empty2, node.Nodes[2]);
        }
    }
}
