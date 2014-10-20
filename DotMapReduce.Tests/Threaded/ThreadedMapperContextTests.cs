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
		public void Threaded_Emit_Partition_Success()
		{
			//Arrange
			var num = 100;
			var workers = 10;
			var context = new ThreadedMapperContext(workers);
			var tasks = new List<Task>();

			for (var i = 0; i < 100; i++)
			{
				tasks.Add(context.EmitAsync((i % (num / 3)).ToString(), "1"));
			}

			Task.WaitAll(tasks.ToArray());

			//Act
			var partitionResults = Enumerable.Range(0, 10).Select(i => context.GetPartitionedEmittedValues(i)).ToList();
			var groupingResults = partitionResults.SelectMany(g => g).ToList();
			var individualResults = groupingResults.SelectMany(g => g).ToList();

			//Assert
			Assert.That(partitionResults.Count, Is.EqualTo(workers));
			Assert.That(groupingResults.Count, Is.EqualTo(num / 3));
			Assert.That(individualResults.Count, Is.EqualTo(num));
		}
	}
}
