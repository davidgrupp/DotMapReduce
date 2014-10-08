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
	public class ThreadedMapReduceWorkerTests
	{
		[SetUp]
		public void Setup()
		{
			_fileService = MockFileSystem.Setup();
		}

		private Mock<IMapReduceFileService> _fileService;
		
		[Test]
		public void asdf()
		{
			//Arrange
			var mapper = new WordCountMapper();

			//Act
			var worker = new ThreadedMapReduceWorker(3, 10, null, _fileService.Object, mapper);

			//Assert
		}
	}
}
