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
	public class GenericRunnerTests
	{
		[Test, Category("Unit")]
		public void GenericRunner_Run_Success()
		{
			//Arrange
			var mapper = new WordCountMapper();
			var reducer = new WordCountReducer();
			var fileService = MockFileSystem.SetupMappers();

			//Act
			var runner = new GenericMapReduceRunner(mapper, reducer, fileService.Object);
			runner.Run(MockFileSystem.InputDirectory, "OutDir");

			//Assert
			MockFileSystem.VerifyMappers(fileService);
			MockFileSystem.VerifyReducers(fileService);
		}
	}
}
