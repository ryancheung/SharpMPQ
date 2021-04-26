// MIT License - Copyright (C) ryancheung
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using System;
using System.IO;
using System.Runtime.InteropServices;
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

        [TestMethod]
        public void Testlibmpq__archive_open_error()
        {
            IntPtr mpq = default;
            var result = libmpq__archive_open(ref mpq, "", -1);

            Assert.AreEqual(LIBMPQ_ERROR_OPEN, result);
        }

        [TestMethod]
        public void Testlibmpq__archive_open_bad_format()
        {
            IntPtr mpq = default;
            var result = libmpq__archive_open(ref mpq, Path.Combine(AppContext.BaseDirectory, "SharpMPQ.dll"), -1);

            Assert.AreEqual(LIBMPQ_ERROR_FORMAT, result);
        }

        [TestMethod]
        public void Testlibmpq__archive_open_success()
        {
            IntPtr mpq = default;
            var result = libmpq__archive_open(ref mpq, Path.Combine(AppContext.BaseDirectory, "dummy.MPQ"), -1);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Testlibmpq__archive_close()
        {
            IntPtr mpq = default;
            var result = libmpq__archive_open(ref mpq, Path.Combine(AppContext.BaseDirectory, "dummy.MPQ"), -1);

            result = libmpq__archive_close(mpq);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Testlibmpq__file_number()
        {
            var mpqFilename = Path.Combine(AppContext.BaseDirectory, "dummy.MPQ");
            IntPtr mpq = default;
            var result = libmpq__archive_open(ref mpq, mpqFilename, -1);

            uint fileNum;
            string dbcFileName = "DBFilesClient\\Map.dbc";
            result = libmpq__file_number(mpq, dbcFileName, out fileNum);

            Assert.AreEqual(0, result);
            Assert.IsTrue(fileNum > 0);
        }

        [TestMethod]
        public void Testlibmpq__file_size_unpacked()
        {
            var mpqFilename = Path.Combine(AppContext.BaseDirectory, "dummy.MPQ");
            IntPtr mpq = default;
            var result = libmpq__archive_open(ref mpq, mpqFilename, -1);

            uint fileNum;
            string dbcFileName = "DBFilesClient\\Map.dbc";
            result = libmpq__file_number(mpq, dbcFileName, out fileNum);

            long size;
            result = libmpq__file_size_unpacked(mpq, fileNum, out size);

            Assert.AreEqual(0, result);
            Assert.IsTrue(size > 0);
        }

        [TestMethod]
        public void Testlibmpq__file_read()
        {
            var mpqFilename = Path.Combine(AppContext.BaseDirectory, "dummy.MPQ");
            IntPtr mpq = default;
            var result = libmpq__archive_open(ref mpq, mpqFilename, -1);

            uint fileNum;
            string dbcFileName = "DBFilesClient\\Map.dbc";
            result = libmpq__file_number(mpq, dbcFileName, out fileNum);

            long size = 0;
            result = libmpq__file_size_unpacked(mpq, fileNum, out size);

            long transferred = 0;
            var buffer = Marshal.AllocHGlobal((int)size);
            result = libmpq__file_read(mpq, fileNum, buffer, size, out transferred);
            Marshal.FreeHGlobal(buffer);

            Assert.AreEqual(0, result);
            Assert.AreEqual(transferred, size);
        }
    }
}
