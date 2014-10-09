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
	public class ThreadedMapReduceManagerTests
	{
		[SetUp]
		public void Setup()
		{
			_dataExchanger = new Mock<IDataExchanger>();
		}

		private Mock<IDataExchanger> _dataExchanger;

		[Test, Category("Unit")]
		public void Threaded_RunMappers_Success()
		{
			//Arrange
			var keys = MockFileSystem.GetKeys();
			var worker = new Mock<IMapReduceWorker>();
			worker.Setup(w => w.RunMapperBatchAsync(MockFileSystem.InputDirectory, It.IsAny<List<String>>())).Returns(Task.Delay(2000));
			var workers = new List<IMapReduceWorker>() { worker.Object };
			var manager = new ThreadedMapReduceManager(_dataExchanger.Object);
			manager.Workers.AddRange(workers);

			//Act
			manager.RunMappers(MockFileSystem.InputDirectory, keys);

			//Assert
			worker.Verify(w => w.RunMapperBatchAsync(MockFileSystem.InputDirectory, It.IsAny<List<String>>()));
		}


		[Test, Category("Unit")]
		[ExpectedException(ExpectedException = typeof(ArgumentException), ExpectedMessage = "Workers empty. Cannot run mappers without workers.")]
		public void Threaded_RunMappers_NoWorkers()
		{
			//Arrange
			var keys = MockFileSystem.GetKeys();

			//Act
			var manager = new ThreadedMapReduceManager(_dataExchanger.Object);
			manager.RunMappers(MockFileSystem.InputDirectory, keys);

			//Assert

		}
	}
}
