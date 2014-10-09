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
	[TestFixture, Category("Core"), Category("Threaded")]
	public class ThreadedMapReduceWorkerTests
	{
		[SetUp]
		public void Setup()
		{
			_fileService = new Mock<IMapReduceFileService>();
			_manager = new Mock<IMapReduceManager>();
		}

		private Mock<IMapReduceFileService> _fileService;
		private Mock<IMapReduceManager> _manager;

		[Test, Category("Unit")]
		public void Threaded_RunMapperBatch_Success()
		{
			//Arrange
			var keys = MockFileSystem.GetKeys();
			MockFileSystem.SetupMappers(_fileService);
			var mapper = new WordCountMapper();

			//Act
			var worker = new ThreadedMapReduceWorker(3, 10, _manager.Object, _fileService.Object, mapper);
			worker.RunMapperBatchAsync(MockFileSystem.InputDirectory, keys).Wait();

			//Assert
			MockFileSystem.VerifyMappers(_fileService);
		}

		[Test, Category("Unit")]
		public void Threaded_ExchangeKeyValues_Success()
		{
			//Arrange
			var keys = MockFileSystem.GetKeys();
			MockFileSystem.SetupMappers(_fileService);
			var mapper = new WordCountMapper();

			var otherWrkContext = new Mock<IMapperContext>();
			otherWrkContext.Setup(c => c.GetPartitionedEmittedValues(3)).Returns(new Dictionary<String, List<String>>());
			var context = new Mock<IMapperContext>();
			context.Setup(c => c.GetPartitionedEmittedValues(4)).Returns(new Dictionary<String, List<String>>());

			var otherWorker = new ThreadedMapReduceWorker(4, 10, _manager.Object, _fileService.Object, mapper, otherWrkContext.Object, null);

			//Act
			var worker = new ThreadedMapReduceWorker(3, 10, _manager.Object, _fileService.Object, mapper, context.Object, null);
			worker.ExchangeKeyValues(otherWorker);

			//Assert

		}
	}
}
