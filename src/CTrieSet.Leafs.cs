using System.Diagnostics;
using System.Numerics;

namespace CTrie
{
    public partial class CTrieSet<T> : Node
    {
        #region Initialization

        private static INode[] FillRootArray()
        {
            Node empty = new Node();

            return new INode[]
            {
                empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,
                empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,
                empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,
                empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,empty,
            };
        }

        #endregion


        #region Split Leaf

        protected virtual Node GetSplitNode(Leaf one, Leaf two)
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
                return new Node((ulong)1 << shift, new INode[] { GetSplitNode(one, two) });
            }

            // Create split entry
            var array = hashOne > hashTwo
                      ? new INode[] { two, one }
                      : new INode[] { one, two };

            var mask = ((ulong)1 << hashOne) | ((ulong)1 << hashTwo);

            return new Node(mask, mask, array);
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


            #region Implementation

            public void Rotate() => Hash = BitOperations.RotateLeft(Hash, SIZE);

            public override string ToString() => $"Hash = { Hash }, Value = { Value }";

            #endregion
        }

        #endregion
    }
}
