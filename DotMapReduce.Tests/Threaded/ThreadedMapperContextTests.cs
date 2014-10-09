using DotMapReduce.Threaded;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests.Threaded
{
	[TestFixture, Category("Core"), Category("Threaded")]
	public class ThreadedMapperContextTests
	{
		[Test, Category("Unit")]
		public void Threaded_Parition_Success()
		{
			//Arrange
			var context = new ThreadedMapperContext(10);
			for (var i = 0; i < 100; i++)
			{
				context.Emit(i.ToString(), "1");
			}

			//Act
			var results = Enumerable.Range(0, 10).Select(i => context.GetPartitionedEmittedValues(i)).ToList();

			//Assert
			Assert.That(results.All(r=>r.Any()), Is.True);
		}
	}
}
