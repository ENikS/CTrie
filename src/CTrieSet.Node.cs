using System;
using System.Diagnostics;
using System.Numerics;

namespace CTrie
{
    #region INode interface

    public interface INode
    {
    }

    #endregion


    #region Node

    [DebuggerDisplay("Nodes = {Nodes.Length}")]
    public class Node : INode
    {
        public ulong Leafs;
        public ulong Flags;
        public INode[] Nodes;

        public Node()
        {
            Nodes = Array.Empty<INode>();
        }

        protected Node(INode[] nodes)
        {
            Flags = ulong.MaxValue;
            Leafs  = ulong.MinValue;
            Nodes = nodes;
        }

        public Node(ulong bitmap, INode[] nodes)
        {
            Flags = bitmap;
            Nodes = nodes;
        }

        public Node(ulong bitmap, ulong leafs, INode[] nodes)
        {
            Flags = bitmap;
            Leafs = leafs;
            Nodes = nodes;
        }

        public Node(ulong position, INode leaf, Node node)
        {
            Flags = node.Flags  | position;
            Leafs  = node.Leafs | position;
            
            Nodes = new INode [node.Nodes.Length + 1];

            var length = node.Nodes.Length;
            var index = BitOperations.PopCount(position - 1 & Flags);
            
            Nodes[index] = leaf;
            
            if (0 < index) Array.Copy(node.Nodes, Nodes, index);
            if (length > index) Array.Copy(node.Nodes, index, Nodes, index + 1, length - index);
        }

        public override string ToString() => $"Nodes = {Nodes.Length}, Bitmap = {Flags:X16}, Leafs = {Leafs:X16}";
    }

    #endregion
}
