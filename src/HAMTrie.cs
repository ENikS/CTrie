using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace HAMT
{
    public partial class HAMTrie<T> : TrieNode
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

        public HAMTrie()
        {
            Nodes = new INode[BUCKET_SIZE];
            Bitmap = 0xFFFFFFFFFFFFFFFF;
            Array.Fill(Nodes, new TrieNode());
        }

        #endregion


        public int Length => _count;


        public ref T this[uint hash]
        {
            get 
            {
                Leaf leaf;
                TrieNode childNode;
                TrieNode parentNode = this;
                TrieNode activeNode = this;
                INode formerNode;


                do
                {
                    int index = 0;
                    var shift = hash;
                    var position = (ulong)1 << (int)(hash & MASK);

                    int parentIndex = 0;

                    while ((activeNode.Bitmap & position) != 0)
                    {
                        index = BitOperations.PopCount(position - 1 & activeNode.Bitmap);

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
                            activeNode = (TrieNode)formerNode;
                            continue;
                        }

                        parentNode = activeNode;
                        parentIndex = index;
                        activeNode = (TrieNode)activeNode.Nodes[index];

                        shift = shift >> SIZE;
                        position = (ulong)1 << (int)(shift & MASK);
                    }

                    // Calculate metadata
                    var flags = activeNode.Bitmap | position;
                    var length = activeNode.Nodes.Length;
                    var nodes = new INode[length + 1];
                    var point = BitOperations.PopCount(position - 1 & flags);

                    // Copy other entries if required
                    if (0 < point) Array.Copy(activeNode.Nodes, nodes, point);
                    if (length > point) Array.Copy(activeNode.Nodes, point, nodes, point + 1, length - point);

                    // Add leaf node to array
                    leaf = new Leaf(shift);
                    nodes[point] = leaf;
                    childNode  = new TrieNode(flags, activeNode.Leafs | position, nodes);
                    formerNode = Interlocked.CompareExchange(ref parentNode.Nodes[index], childNode, activeNode);

                } while (false == ReferenceEquals(formerNode, activeNode));

                Interlocked.Increment(ref _count);

                return ref leaf.Value; 
            }
        }
    }
}
