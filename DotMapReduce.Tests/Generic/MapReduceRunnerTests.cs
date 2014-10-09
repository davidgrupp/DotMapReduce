using DotMapReduce.Generic;
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
	[TestFixture, Category("Core"), Category("Generic")]
	public class MapReduceRunnerTests
	{
		[Test, Category("Unit")]
		public void MapReduceRunner_Run_Success()
		{
			//Arrange
			var mapper = new WordCountMapper();
			var reducer = new WordCountReducer();
			var fileService = MockFileSystem.Setup();

			//Act
			var runner = new GenericMapReduceRunner(mapper, reducer, fileService.Object);
			runner.Run("TestDir", "OutDir");

			//Assert
			MockFileSystem.Verify(fileService);
		}
	}
}
