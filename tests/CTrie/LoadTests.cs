using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace CTrie.Tests
{
    [TestClass]
    public class LoadTests
    {
        static int SizeTypes = 2000;
        static Type[] TestTypes = Assembly.GetAssembly(typeof(int))
                                                    .DefinedTypes
                                                    .Where(t => t != typeof(IServiceProvider))
                                                    .Take(SizeTypes)
                                                    .ToArray();
        static uint[] Codes = TestTypes.Select(t => (uint)t.GetHashCode())
                                .Distinct()
                                .ToArray();

        CTrieSet<Type> Trie = new CTrieSet<Type>();
        ConcurrentDictionary<uint, Type> Dictionary = new ConcurrentDictionary<uint, Type>();

        [TestInitialize]
        public void InitializeTest()
        {
            foreach (var type in TestTypes)
            {
                var hash = (uint)type.GetHashCode();

                Trie[hash] = type;
                Dictionary[hash] = type;
            }
        }

        [TestMethod]
        public void AccessCTrieSet()
        {
            foreach (var hash in Codes)
            {
                var value = Trie[hash];
                Assert.IsNotNull(value);
            }
        }

        [TestMethod]
        public void AccessConcurrentDictionary()
        {
            foreach (var hash in Codes)
            {
                var value = Dictionary[hash];
                Assert.IsNotNull(value);
            }
        }


        [TestMethod]
        public void LoadCTrieSet()
        {
            var trie = new CTrieSet<Type>();
            uint hash = 0;

            foreach (var type in TestTypes)
            {
                hash = (uint)type.GetHashCode();

                trie[hash] = type;
            }

            Assert.IsNotNull(trie[hash]);
        }


        [TestMethod]
        public void LoadConcurrentDictionary()
        {
            var set = new ConcurrentDictionary<uint, Type>();
            uint hash = 0;

            foreach (var type in TestTypes)
            {
                hash = (uint)type.GetHashCode();

                set[hash] = type;
            }

            Assert.IsNotNull(set[hash]);
        }
    }
}
