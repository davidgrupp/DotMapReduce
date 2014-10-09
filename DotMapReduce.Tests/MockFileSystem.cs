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
		public const String InputDirectory = "TestDir";
		public const String OutputDirectory = "OutDir";

		public static List<String> GetKeys()
		{
			var keys = new List<String>();

			for (var i = 0; i < 16 * 15; i += 3)
			{
				keys.AddRange(new List<String>() { "Key" + i, "Key" + (i + 1), "Key" + (i + 2) });
			}

			return keys;
		}

		public static Mock<IMapReduceFileService> SetupMappers()
		{
			var fileService = new Mock<IMapReduceFileService>();

			SetupMappers(fileService);

			return fileService;
		}

		public static void SetupMappers(Mock<IMapReduceFileService> fileService)
		{
			var keys = new List<String>();

			for (var i = 0; i < 16 * 15; i += 3)
			{
				keys.AddRange(new List<String>() { "Key" + i, "Key" + (i + 1), "Key" + (i + 2) });
				fileService.Setup(fs => fs.ReadDocument(InputDirectory, "Key" + i)).Returns("This is document one");
				fileService.Setup(fs => fs.ReadDocument(InputDirectory, "Key" + (i + 1))).Returns("This is document two");
				fileService.Setup(fs => fs.ReadDocument(InputDirectory, "Key" + (i + 2))).Returns("This is document three");
			}
			fileService.Setup(fs => fs.ReadDocumentIds(InputDirectory)).Returns(keys);
		}

		public static void VerifyMappers(Mock<IMapReduceFileService> fileService)
		{
			fileService.Verify(fs => fs.ReadDocument(InputDirectory, It.IsAny<String>()), Times.Exactly(16 * 15));
		}

		public static Dictionary<String, List<String>> GetReducerData()
		{
			Dictionary<String, List<String>> data = new Dictionary<String, List<String>>();
			data.Add("This", Enumerable.Range(0, 240).Select(i => "1").ToList());
			data.Add("is", Enumerable.Range(0, 240).Select(i => "1").ToList());
			data.Add("document", Enumerable.Range(0, 240).Select(i => "1").ToList());
			data.Add("one", Enumerable.Range(0, 80).Select(i => "1").ToList());
			data.Add("two", Enumerable.Range(0, 80).Select(i => "1").ToList());
			data.Add("three", Enumerable.Range(0, 80).Select(i => "1").ToList());
			return data;
		}

		public static void SetupReducers(Mock<IMapReduceFileService> fileService)
		{
			fileService.Setup(fs => fs.WriteToDocument(It.IsAny<String>(), "This: 240"));
			fileService.Setup(fs => fs.WriteToDocument(It.IsAny<String>(), "is: 240"));
			fileService.Setup(fs => fs.WriteToDocument(It.IsAny<String>(), "document: 240"));
			fileService.Setup(fs => fs.WriteToDocument(It.IsAny<String>(), "one: 80"));
			fileService.Setup(fs => fs.WriteToDocument(It.IsAny<String>(), "two: 80"));
			fileService.Setup(fs => fs.WriteToDocument(It.IsAny<String>(), "three: 80"));
		}

		public static void VerifyReducers(Mock<IMapReduceFileService> fileService)
		{
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "This: 240"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "is: 240"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "document: 240"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "one: 80"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "two: 80"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "three: 80"));
		}
	}
}
