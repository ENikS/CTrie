using System.Diagnostics;
using System.Numerics;

namespace HAMT
{
    public partial class HAMTrie<T> : TrieNode
    {
        #region Initialization

        private static ITrieNode[] FillRootArray()
        {
            TrieNode empty = new TrieNode();

            return new ITrieNode[]
            {
                empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,
                empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,
                empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,
                empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,
            };
        }

        #endregion


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
                return new TrieNode((ulong)1 << shift, new ITrieNode[] { GetSplitNode(one, two) });
            }

            // Create split entry
            var array = hashOne > hashTwo
                      ? new ITrieNode[] { two, one }
                      : new ITrieNode[] { one, two };

            var mask = ((ulong)1 << hashOne) | ((ulong)1 << hashTwo);

            return new TrieNode(mask, mask, array);
        }

        #endregion


        #region Nested

        [DebuggerDisplay("Leaf: { Value }")]
        public class Leaf : ITrieNode
        {
            #region Fields

            public uint Hash;
            public T Value = default!;

            #endregion


            #region Constructors

            public Leaf(uint hash) => Hash = hash;


            #endregion


            #region Implementation

            public void Rotate() => Hash = BitOperations.RotateLeft(Hash, SIZE);

            public override string ToString() => $"Hash = { Hash }, Value = { Value }";

            #endregion
        }

        #endregion
    }
}
