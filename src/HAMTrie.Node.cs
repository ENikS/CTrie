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

    [DebuggerDisplay("Nodes = {Nodes.Length}")]
    public class TrieNode : INode
    {
        public ulong Bitmap;
        public INode[] Nodes;

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

        public override string ToString() => $"Nodes = {Nodes.Length}, Bitmap = {Bitmap:X16}";
    }

    #endregion
}
