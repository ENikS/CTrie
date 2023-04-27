using System.Diagnostics;
using System.Numerics;

namespace HAMT
{
    public partial class HAMTrie<T> : TrieNode
    {

        #region Split Leaf

        protected virtual TrieNode GetSplitNode(Leaf one, Leaf two)
        {
            // Calculate new hash
            var hashOne = (int)(one.Hash & MASK);
            var hashTwo = (int)(two.Hash & MASK);

            // Branch into new level
            if (hashOne == hashTwo)
            {
                var shift = (int)(one.Hash & MASK);
                one.Rotate();
                two.Rotate();
                return new TrieNode((ulong)1 << shift, new INode[] { GetSplitNode(one, two) });
            }

            // Create split entry
            var array = hashOne > hashTwo
                      ? new INode[] { two, one }
                      : new INode[] { one, two };

            var mask = ((ulong)1 << hashOne) | ((ulong)1 << hashTwo);

            return new TrieNode(mask, array);
        }

        #endregion


        #region Nested

        [DebuggerDisplay("Leaf: { Value }")]
        public class Leaf : INode
        {
            #region Fields

            public uint Hash;
            public T Value = default!;

            #endregion


            #region Constructors

            public Leaf(uint hash) => Hash = hash;


            #endregion

            public bool IsLeaf => true;


            #region Implementation

            public void Rotate() => Hash = Hash >> SIZE;

            public override string ToString() => $"Hash = { Hash }, Value = { Value }";

            #endregion
        }

        #endregion
    }
}
