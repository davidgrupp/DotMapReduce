using DotMapReduce.Interfaces;
using DotMapReduce.Parallelization.Threaded;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests.Parallelization.Threaded
{
	[TestFixture]
	public class ThreadedMapReduceRunnerTests
	{
		[Test]
		public void ThreadedRunner_Run_Success()
		{
			//Arrange
			var mapper = new WordCountMapper();
			var reducer = new WordCountReducer();
			var fileService = MockFileSystem.Setup();

			//Act
			var runner = new ThreadedMapReduceRunner(mapper, reducer, fileService.Object);
			runner.Run("TestDir", "OutDir");

			//Assert
			MockFileSystem.Verify(fileService);
		}
	}
}
