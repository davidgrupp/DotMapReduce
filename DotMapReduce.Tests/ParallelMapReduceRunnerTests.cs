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
	public class ParallelMapReduceRunnerTests
	{
		[Test]
		public void Parallel_Runner_Success()
		{
			//Arrange
			var mapper = new WordCountMapper();
			var reducer = new WordCountReducer();
			var fileService = new Mock<IMapReduceFileService>();

			fileService.Setup(fs => fs.ReadDocumentIds("TestDir")).Returns(new List<String>() { "One", "Two", "Three" });
			fileService.Setup(fs => fs.ReadDocument("TestDir", "One")).Returns("This is document one");
			fileService.Setup(fs => fs.ReadDocument("TestDir", "Two")).Returns("This is document two");
			fileService.Setup(fs => fs.ReadDocument("TestDir", "Three")).Returns("This is document three");

			//Act
			var runner = new MapReduceRunner(mapper, reducer, fileService.Object);
			runner.Run("TestDir", "OutDir");

			//Assert
			
		}

		
	}
}
