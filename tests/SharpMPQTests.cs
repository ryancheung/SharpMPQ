using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpMPQ.Tests
{
    using static NativeMethods;

    [TestClass]
    public class SharpMPQTests
    {
        [TestMethod]
        public void Testlibmpq__version()
        {
            var version = libmpq__version();
            Assert.AreEqual("0.4.2", version);
        }

        [TestMethod]
        public void Testlibmpq__strerror()
        {
            var err = libmpq__strerror(-1);
            Assert.AreEqual("open error on file", err);
        }
    }
}
