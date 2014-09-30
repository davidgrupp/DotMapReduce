using DotMapReduce.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests
{
	[TestFixture]
	public class MapReduceRunnerTests
	{
		[Test]
		public void Runner_Success()
		{
			//Arrange
			var mapper = new WordCountMapper();
			var reducer = new WordCountReducer();
			var fileService = new Mock<IMapReduceFileService>();

			fileService.Setup(fs => fs.ReadDocumentIds("TestDir")).Returns(new List<String>() { "One", "Two", "Three" });
			fileService.Setup(fs => fs.ReadDocument("One")).Returns("This is document one");
			fileService.Setup(fs => fs.ReadDocument("Two")).Returns("This is document two");
			fileService.Setup(fs => fs.ReadDocument("Three")).Returns("This is document three");

			//Act
			var runner = new MapReduceRunner(mapper, reducer, fileService.Object);
			runner.Run("TestDir", "OutDir");

			//Assert
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "This: 3"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "is: 3"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "document: 3"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "one: 1"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "two: 1"));
			fileService.Verify(fs => fs.WriteToDocument(It.IsAny<String>(), "three: 1"));
		}
	}
}
