using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace CTrie.Tests
{
    public partial class CTrieTests
    {
        [TestMethod]
        public void RootConflict()
        {
            var data = "Zero";
            var data1 = "First";
            var data2 = "Second";
            using var masterEvnt = new ManualResetEvent(false);
            using var threadEvnt1 = new ManualResetEvent(false);
            using var threadEvnt2 = new ManualResetEvent(false);

            var trie = new TestSet<string>(masterEvnt);

            var hash = (uint)data.GetHashCode();
            var hash1 = hash + 1;
            var hash2 = hash + 2;

            Thread thread1 = new Thread(delegate ()
            {
                threadEvnt1.Set();
                trie[hash1] = data1;
            })
            { Name = "1" };

            Thread thread2 = new Thread(delegate ()
            {
                threadEvnt2.Set();
                trie[hash2] = data2;
            })
            { Name = "2" };

            thread1.Start();
            thread2.Start();

            WaitHandle.WaitAll(new[] { threadEvnt1, threadEvnt2 });

            Thread.Sleep(100);
            Assert.IsTrue(masterEvnt.Set());

            thread1.Join();
            thread2.Join();

            Assert.AreSame(data1, trie[hash1]);
            Assert.AreSame(data2, trie[hash2]);

        }

        [TestMethod]
        public void AccessConflict()
        {
            var data = "Zero";
            var data1 = "First";
            var data2 = "Second";
            using var masterEvnt = new ManualResetEvent(true);
            using var threadEvnt1 = new ManualResetEvent(false);
            using var threadEvnt2 = new ManualResetEvent(false);

            var trie = new TestSet<string>(masterEvnt);

            var hash = (uint)data.GetHashCode();
            var hash1 = hash + 1;
            var hash2 = hash + 2;

            trie[hash] = data;

            Thread thread1 = new Thread(delegate ()
            {
                threadEvnt1.Set();
                trie[hash1] = data1;
            })
            { Name = "1" };

            Thread thread2 = new Thread(delegate ()
            {
                threadEvnt2.Set();
                trie[hash2] = data2;
            })
            { Name = "2" };

            thread1.Start();
            thread2.Start();

            WaitHandle.WaitAll(new [] { threadEvnt1, threadEvnt2 });

            Thread.Sleep(100);
            Assert.IsTrue( masterEvnt.Set());

            thread1.Join();
            thread2.Join();

            Assert.AreSame(data1, trie[hash1]);
            Assert.AreSame(data2, trie[hash2]);

        }

        public class TestSet<T> : CTrieSet<T>
        {
            ManualResetEvent _event;

            public TestSet(ManualResetEvent manual)
            {
                _event = manual;
            }


            protected override Node GetSplitNode(Leaf one, Leaf two)
            {
                _event.WaitOne();
                return base.GetSplitNode(one, two);
            }
        }
    }
}
