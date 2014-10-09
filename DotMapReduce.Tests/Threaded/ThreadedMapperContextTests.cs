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
		public void Threaded_Partition_Success()
		{
			//Arrange
			var context = new ThreadedMapperContext(10);
			var tasks = new List<Task>();
			var num = 100;
			for (var i = 0; i < 100; i++)
			{
				tasks.Add(context.EmitAsync((i % (num / 3)).ToString(), "1"));
			}

			Task.WaitAll(tasks.ToArray());

			//Act
			var results = Enumerable.Range(0, 10).Select(i => context.GetPartitionedEmittedValues(i)).ToList();

			//Assert
			Assert.That(results.All(r => r.Any()), Is.True);
		}
	}
}
