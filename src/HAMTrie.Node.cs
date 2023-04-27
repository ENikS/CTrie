using System;
using System.Diagnostics;

namespace HAMT
{
    #region INode interface

    public interface INode
    {
    }

    #endregion


    #region Node

    [DebuggerDisplay("Nodes = {Nodes.Length}")]
    public class TrieNode : INode
    {
        public ulong Leafs;
        public ulong Bitmap;
        public INode[] Nodes;

        public TrieNode()
        {
            Nodes = Array.Empty<INode>();
        }

        protected TrieNode(INode[] nodes)
        {
            Bitmap = ulong.MaxValue;
            Leafs  = ulong.MinValue;
            Nodes = nodes;
        }

        public TrieNode(ulong bitmap, INode[] nodes)
        {
            Bitmap = bitmap;
            Nodes = nodes;
        }

        public TrieNode(ulong bitmap, ulong leafs, INode[] nodes)
        {
            Bitmap = bitmap;
            Leafs = leafs;
            Nodes = nodes;
        }

        public override string ToString() => $"Nodes = {Nodes.Length}, Bitmap = {Bitmap:X16}, Leafs = {Leafs:X16}";
    }

    #endregion
}
