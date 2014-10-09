using DotMapReduce.Interfaces;
using DotMapReduce.Interfaces.Parallelization;
using DotMapReduce.Threaded;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests.Threaded
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
			Mock<IWorkerExchanger> workerExchanger = new Mock<IWorkerExchanger>();

			//Act
			var runner = new ThreadedMapReduceRunner(mapper, reducer, fileService.Object, workerExchanger.Object);
			runner.Run("TestDir", "OutDir");

			//Assert
			MockFileSystem.Verify(fileService);
		}
	}
}
