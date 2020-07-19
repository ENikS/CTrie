using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace CTrie
{
    public partial class CTrieSet<T> : Node
    {
        #region Constants

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        const int SIZE = 6;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        const int BUCKET_SIZE = 64;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        const uint MASK = BUCKET_SIZE - 1;

        #endregion


        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _count;
        
        #endregion


        #region Constructors

        public CTrieSet()
            : base(FillRootArray())
        {
        }

        #endregion


        public int Length => _count;


        public ref T this[uint hash]
        {
            get 
            {
                Leaf leaf;
                Node childNode;
                Node activeNode;
                Node parentNode;
                INode formerNode;

                int index = 0;

                do
                {
                    int parentIndex = 0;

                    parentNode = this;
                    activeNode = this;

                    var shift = BitOperations.RotateLeft(hash, SIZE);
                    var position = (ulong)1 << (int)(shift & MASK);

                    while ((activeNode.Flags & position) != 0)
                    {
                        index = BitOperations.PopCount(position - 1 & activeNode.Flags);

                        if ((activeNode.Leafs & position) != 0)
                        {
                            leaf = (Leaf)activeNode.Nodes[index];

                            if (leaf.Hash == shift) return ref leaf.Value;

                            var other = new Leaf(shift);
                            //var subst = new Node()
                            formerNode = Interlocked.CompareExchange(ref parentNode.Nodes[parentIndex],
                                GetSplitNode(leaf, other), activeNode);

                            // Return on success
                            if (true == ReferenceEquals(formerNode, activeNode))
                            {
                                Interlocked.Increment(ref _count);
                                return ref other.Value;
                            }

                            // Roll back and start over if failed
                            activeNode = (Node)formerNode;
                            continue;
                        }

                        parentNode = activeNode;
                        parentIndex = index;
                        activeNode = (Node)activeNode.Nodes[index];

                        shift = BitOperations.RotateLeft(shift, SIZE);
                        position = (ulong)1 << (int)(shift & MASK);
                    }

                    // Calculate metadata
                    var flags = activeNode.Flags | position;
                    var length = activeNode.Nodes.Length;
                    var nodes = new INode[length + 1];
                    var point = BitOperations.PopCount(position - 1 & flags);

                    // Copy other entries if required
                    if (0 < point) Array.Copy(activeNode.Nodes, nodes, point);
                    if (length > point) Array.Copy(activeNode.Nodes, point, nodes, point + 1, length - point);

                    // Add leaf node to array
                    leaf = new Leaf(shift);
                    nodes[point] = leaf;
                    childNode  = new Node(flags, activeNode.Leafs | position, nodes);
                    formerNode = Interlocked.CompareExchange(ref parentNode.Nodes[index], childNode, activeNode);

                } while (false == ReferenceEquals(formerNode, activeNode));

                Interlocked.Increment(ref _count);

                return ref leaf.Value; 
            }
        }
    }
}
