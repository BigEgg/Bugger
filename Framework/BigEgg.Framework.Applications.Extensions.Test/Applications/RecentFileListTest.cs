using BigEgg.Framework.Applications.Extensions.Applications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications
{
    [TestClass]
    public class RecentFileListTest
    {
        [TestMethod]
        public void SetMaxFilesNumberTest()
        {
            var recentFileList = new RecentFileList();
            recentFileList.AddFile("Doc4");
            recentFileList.AddFile("Doc3");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc1");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc1", "Doc2", "Doc3", "Doc4" }));

            // Set a lower number than items are in the list => expect that the list is truncated.
            recentFileList.MaxFilesNumber = 3;
            Assert.AreEqual(3, recentFileList.MaxFilesNumber);
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc1", "Doc2", "Doc3" }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetMaxFilesNumberTest_Precondition()
        {
            var recentFileList = new RecentFileList();
            recentFileList.MaxFilesNumber = -3;
        }

        [TestMethod]
        public void AddFilesTest()
        {
            var recentFileList = new RecentFileList();
            recentFileList.MaxFilesNumber = 3;

            // Add files to an empty list
            recentFileList.AddFile("Doc3");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc3" }));
            recentFileList.AddFile("Doc2");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc2", "Doc3" }));
            recentFileList.AddFile("Doc1");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc1", "Doc2", "Doc3" }));

            // Add a file to a full list
            recentFileList.AddFile("Doc4");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc4", "Doc1", "Doc2" }));

            // Add a file that already exists in the list
            recentFileList.AddFile("Doc2");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc2", "Doc4", "Doc1" }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddFilesTest_Precondition()
        {
            var recentFileList = new RecentFileList();
            recentFileList.AddFile(null);
        }

        [TestMethod]
        public void AddFilesAndPinThemTest()
        {
            var recentFileList = new RecentFileList();
            recentFileList.MaxFilesNumber = 3;

            // Add files to an empty list
            recentFileList.AddFile("Doc3");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc1");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc1", "Doc2", "Doc3" }));

            // Pin last file
            recentFileList.RecentFiles.First(r => r.Path == "Doc3").IsPinned = true;
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc3", "Doc1", "Doc2" }));

            // Add a file to a full list
            recentFileList.AddFile("Doc4");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc3", "Doc4", "Doc1" }));

            // Add a file that already exists in the list
            recentFileList.AddFile("Doc1");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc3", "Doc1", "Doc4" }));

            // Pin all files
            recentFileList.RecentFiles.First(r => r.Path == "Doc4").IsPinned = true;
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc4", "Doc3", "Doc1" }));
            recentFileList.RecentFiles.First(r => r.Path == "Doc1").IsPinned = true;
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc1", "Doc4", "Doc3" }));

            // Add a file to a full pinned list
            recentFileList.AddFile("Doc5");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc1", "Doc4", "Doc3" }));

            // Add a file that already exists in the list
            recentFileList.AddFile("Doc4");
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc4", "Doc1", "Doc3" }));

            // Unpin files
            recentFileList.RecentFiles.First(r => r.Path == "Doc4").IsPinned = false;
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc1", "Doc3", "Doc4" }));
            recentFileList.RecentFiles.First(r => r.Path == "Doc1").IsPinned = false;
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "Doc3", "Doc1", "Doc4" }));
        }

        [TestMethod]
        public void XmlSerializingTest_Empty()
        {
            var serializer = new XmlSerializer(typeof(RecentFileList));

            using (var stream = new MemoryStream())
            {
                var recentFileList1 = new RecentFileList();
                serializer.Serialize(stream, recentFileList1);

                stream.Position = 0;
                var recentFileList2 = (RecentFileList)serializer.Deserialize(stream);

                Assert.AreEqual(recentFileList1.RecentFiles.Count, recentFileList2.RecentFiles.Count);
                Assert.IsTrue(recentFileList1.RecentFiles.Select(f => f.Path).SequenceEqual(recentFileList2.RecentFiles.Select(f => f.Path)));
            }
        }

        [TestMethod]
        public void XmlSerializingTest_HaveItem()
        {
            var serializer = new XmlSerializer(typeof(RecentFileList));

            using (var stream = new MemoryStream())
            {
                var recentFileList1 = new RecentFileList();
                recentFileList1.AddFile("Doc3");
                recentFileList1.AddFile("Doc2");
                recentFileList1.AddFile("Doc1");
                serializer.Serialize(stream, recentFileList1);

                stream.Position = 0;
                var recentFileList2 = (RecentFileList)serializer.Deserialize(stream);

                Assert.IsTrue(recentFileList1.RecentFiles.Select(f => f.Path).SequenceEqual(recentFileList2.RecentFiles.Select(f => f.Path)));
            }
        }

        [TestMethod]
        public void GetSchemaTest()
        {
            var recentFileList = new RecentFileList();
            IXmlSerializable serializable = recentFileList;
            Assert.IsNull(serializable.GetSchema());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadXmlTest()
        {
            var recentFileList = new RecentFileList();
            IXmlSerializable serializable = recentFileList;
            serializable.ReadXml(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteXmlTest()
        {
            var recentFileList = new RecentFileList();
            IXmlSerializable serializable = recentFileList;
            serializable.WriteXml(null);
        }

        [TestMethod]
        public void LoadTest_Empty()
        {
            var recentFileList = new RecentFileList();
            recentFileList.MaxFilesNumber = 3;
            recentFileList.AddFile("Doc3");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc1");

            recentFileList.Load(new RecentFile[] { });
            Assert.IsFalse(recentFileList.RecentFiles.Any());
        }

        [TestMethod]
        public void LoadTest()
        {
            var recentFileList = new RecentFileList();
            recentFileList.MaxFilesNumber = 3;
            recentFileList.AddFile("Doc3");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc1");

            recentFileList.Load(new RecentFile[]
            {
                new RecentFile("NewDoc1") { IsPinned = true },
                new RecentFile("NewDoc2"),
                new RecentFile("NewDoc3"),
                new RecentFile("NewDoc4")
            });
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.Path).SequenceEqual(new[] { "NewDoc1", "NewDoc2", "NewDoc3" }));
            Assert.IsTrue(recentFileList.RecentFiles.Select(f => f.IsPinned).SequenceEqual(new[] { true, false, false }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadTest_Precondition()
        {
            var recentFileList = new RecentFileList();
            recentFileList.Load(null);
        }
    }
}
