using System;
using System.Diagnostics;

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

        public override string ToString() => $"Nodes = {Nodes.Length}, Bitmap = {Flags:X16}, Leafs = {Leafs:X16}";
    }

    #endregion
}
