﻿using PCLExt.FileStorage.Folders;

#if WINDOWS_UWP
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif


namespace PCLExt.FileStorage.Test
{
#if WINDOWS_UWP
    [TestClass]
#else
    [TestFixture]
#endif
    public class StandardFoldersTest
    {
#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void FolderFromPath()
        {
            var folder = new FolderFromPath(new TestFolder().Path);
            var folder1 = new TestFolder();
            Assert.IsTrue(folder.Path == folder1.Path);

            folder.Delete();
        }

#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void LocalStorageFolder() =>
#if __ANDROID__ || __IOS__
            Assert.IsTrue(new LocalRootFolder().Exists);
#else
            Assert.IsTrue(new LocalRootFolder().Exists);
#endif


#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void RoamingStorageFolder() =>
#if __ANDROID__ || __IOS__
            Assert.IsFalse(new RoamingRootFolder().Exists);
#else
            Assert.IsTrue(new RoamingRootFolder().Exists);
#endif

#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void ApplicationFolder() =>
#if __ANDROID__
            Assert.IsFalse(new ApplicationRootFolder().Exists);
#else
            Assert.IsTrue(new ApplicationRootFolder().Exists);
#endif

#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void DocumentsFolder() =>
            //#if __ANDROID__ || __IOS__
            //            Assert.IsFalse(new DocumentsRootFolder().Exists);
            //#else
#if WINDOWS_UWP
            Assert.ThrowsException<System.UnauthorizedAccessException>(() => new DocumentsRootFolder());
#else
             Assert.IsTrue(new DocumentsRootFolder().Exists);
#endif

        //#endif

#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void TempFolder() =>
            Assert.IsTrue(new TempRootFolder().Exists);

#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void TempFolderCreateTemp() =>
            Assert.IsTrue(new TempRootFolder().CreateTempFile().Exists);

#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void TempFolderCreateTempExtension() =>
            Assert.IsTrue(new TempRootFolder().CreateTempFile("ps1").Exists);

#if WINDOWS_UWP
        [TestMethod]
#else
        [Test]
#endif
        public void TempFolderCreateTemp1()
        {
#if WINDOWS_UWP
            Assert.ThrowsException<System.UnauthorizedAccessException>(() => new DocumentsRootFolder());
#else
            var t1 = new DocumentsRootFolder();
#endif
            var t2 = new ApplicationRootFolder();
            var t3 = new LocalRootFolder();
            var t4 = new RoamingRootFolder();
            var t5 = new TempRootFolder();
        }
    }
}