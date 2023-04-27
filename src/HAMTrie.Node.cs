using System;
using System.Diagnostics;

namespace HAMT
{
    #region INode interface

    public interface INode
    {
        bool IsLeaf { get; }
    }

    #endregion


    #region Node

    public class TrieNode : INode
    {
        public INode[] Nodes;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public ulong Bitmap;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsLeaf => false;

        public TrieNode()
        {
            Nodes = Array.Empty<INode>();
        }

        public TrieNode(ulong bitmap, INode[] nodes)
        {
            Bitmap = bitmap;
            Nodes = nodes;
        }

        public override string ToString() => $"Bitmap: {Bitmap:X16},\t Nodes: {Nodes.Length}";
    }

    #endregion
}
