using System;
using System.Diagnostics;

namespace HAMT
{
    #region INode interface

    public interface ITrieNode
    {
    }

    #endregion


    #region Node

    [DebuggerDisplay("Nodes = {Nodes.Length}")]
    public class TrieNode : ITrieNode
    {
        public ulong Leafs;
        public ulong Flags;
        public ITrieNode[] Nodes;

        public TrieNode()
        {
            Nodes = Array.Empty<ITrieNode>();
        }

        protected TrieNode(ITrieNode[] nodes)
        {
            Flags = ulong.MaxValue;
            Leafs  = ulong.MinValue;
            Nodes = nodes;
        }

        public TrieNode(ulong bitmap, ITrieNode[] nodes)
        {
            Flags = bitmap;
            Nodes = nodes;
        }

        public TrieNode(ulong bitmap, ulong leafs, ITrieNode[] nodes)
        {
            Flags = bitmap;
            Leafs = leafs;
            Nodes = nodes;
        }

        public override string ToString() => $"Nodes = {Nodes.Length}, Bitmap = {Flags:X16}, Leafs = {Leafs:X16}";
    }

    #endregion
}
