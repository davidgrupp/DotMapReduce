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

			//Act
			var fileService = new CouchMapReduceFileService();
			var id = fileService.CreateDocument("baseball", "", new { Item = "Orange" });

			//Assert
			Assert.That(id, Is.Not.Null);
		}
	}
}
