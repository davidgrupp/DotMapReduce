using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.CouchDB.Tests
{
	[TestFixture, Category("CouchDB")]
	public class CouchMapReduceFileServiceTests
	{
		[Test, Category("Integration")]
		public void CreateDocument_String()
		{
			//Arrange
			var fileService = new CouchMapReduceFileService();

			//Act
			var id = fileService.CreateDocument("baseball", "", "Apple");

			//Assert
			Assert.That(id, Is.Not.Null);
		}

		[Test, Category("Integration")]
		public void CreateDocument_Empty()
		{
			//Arrange
			var fileService = new CouchMapReduceFileService();

			//Act
			var id = fileService.CreateDocument("baseball", "");

			//Assert
			Assert.That(id, Is.Not.Null);
		}

		[Test, Category("Integration")]
		public void CreateDocument_Object()
		{
			//Arrange
			var fileService = new CouchMapReduceFileService();

			//Act
			var id = fileService.CreateDocument("baseball", "", new { Item = "Orange" });

			//Assert
			Assert.That(id, Is.Not.Null);
		}

		[Test, Category("Integration")]
		public void CreateDirectory()
		{
			//Arrange
			var fileService = new CouchMapReduceFileService();

			//Act
			fileService.CreateDirectory("baseball");

			//Assert
		}

		[Test, Category("Integration")]
		public void ReadDocument()
		{
			//Arrange
			var fileService = new CouchMapReduceFileService();

			//Act
			var id = fileService.CreateDocument("baseball", "");
			var doc = fileService.ReadDocument("baseball", id);

			//Assert
			Assert.That(doc, Is.Not.Null);
		}

		[Test, Category("Integration")]
		public void ReadDocument_Object()
		{
			//Arrange
			var fileService = new CouchMapReduceFileService();
			var testDoc = new { Item = "Strawberry" };

			//Act
			var id = fileService.CreateDocument("baseball", "", testDoc);
			var doc = fileService.ReadDocument<TestDBDocument>("baseball", id);

			//Assert
			Assert.That(doc, Is.Not.Null);
			Assert.That(doc.Id, Is.Not.Null);
			Assert.That(doc.Item, Is.Not.Null);
			Assert.That(doc.Item, Is.EqualTo(testDoc.Item));
		}

		public class TestDBDocument : CouchDbDocument
		{
			public String Item { get; set; }
		}
	}
}
