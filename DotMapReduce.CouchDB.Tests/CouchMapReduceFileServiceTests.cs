using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.CouchDB.Tests
{
	[TestFixture]
	public class CouchMapReduceFileServiceTests
	{
		[Test]
		public void CreateDocument()
		{
			//Arrange
			var fileService = new CouchMapReduceFileService();

			//Act
			var id = fileService.CreateDocument("baseball", "", new { Item = "Orange" });

			//Assert
			Assert.That(id, Is.Not.Null);
		}

		[Test]
		public void CreateDirectory()
		{
			//Arrange
			var fileService = new CouchMapReduceFileService();

			//Act
			fileService.CreateDirectory("basketball");

			//Assert
		}

		[Test]
		public void ReadDocument()
		{

		}
	}
}
