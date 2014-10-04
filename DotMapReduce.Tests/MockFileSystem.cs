using DotMapReduce.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests
{
	public static class MockFileSystem
	{
		public static Mock<IMapReduceFileService> Setup()
		{
			var fileService = new Mock<IMapReduceFileService>();

			var keys = new List<String>();

			for (var i = 0; i < 16 * 30; i += 3)
			{
				keys.AddRange(new List<String>() { "Key" + i, "Key" + (i + 1), "Key" + (i + 2) });
				fileService.Setup(fs => fs.ReadDocument("TestDir", "Key" + i)).Returns("This is document one");
				fileService.Setup(fs => fs.ReadDocument("TestDir", "Key" + (i + 1))).Returns("This is document two");
				fileService.Setup(fs => fs.ReadDocument("TestDir", "Key" + (i + 2))).Returns("This is document three");
			}
			fileService.Setup(fs => fs.ReadDocumentIds("TestDir")).Returns(keys);

			return fileService;
		}

		public static void Verify(Mock<IMapReduceFileService> fileService)
		{
			fileService.Verify(fs => fs.ReadDocument("TestDir", It.IsAny<String>()), Times.Exactly(16 * 30));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "This: 480"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "is: 480"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "document: 480"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "one: 160"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "two: 160"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "three: 160"));
		}
	}
}
