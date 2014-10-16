using DotMapReduce.Interfaces.Parallelization;
using DotMapReduce.Parallelization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests.Parallelization
{
	[TestFixture, Category("Core")]
	public class DataExchangerTests
	{
		[Test, Category("Unit")]
		[TestCase(2), TestCase(3), TestCase(4), TestCase(5), TestCase(6), TestCase(7), TestCase(8), TestCase(9), TestCase(10)]
		[TestCase(99), TestCase(100), TestCase(101)]
		public void GetExchanges(Int32 num)
		{
			Console.WriteLine(num);
			//Arrange
			//var totalExchanges = 0;
			object obj = "";
			var workers = Enumerable.Range(0, num).Select(i => new Mock<IMapReduceWorker>()).ToList();
			var indx = 0;
			var results = new List<Int32>();
			foreach (var worker in workers)
			{
				var w1 = indx++;
				worker.SetupGet(w => w.WorkerId).Returns(w1);
				worker.Setup(w => w.ExchangeKeyValues(It.IsAny<IMapReduceWorker>()))
					.Callback<IMapReduceWorker>((w2) =>
					{
						lock (results) { results.Add(w1); results.Add(w2.WorkerId); }
						//lock (obj) { totalExchanges++; }
					});
			}

			//Act
			var exhngr = new DataExchanger();
			exhngr.ExchangeData(workers.Select(w => w.Object).ToList());

			//Assert
			//verifies that the total number of exchanges is correct
			var expected = Math.Pow(num, 2) - num;//(Math.Pow(num, 2) - num) / 2;
			Assert.That(results.Count(), Is.EqualTo(expected));
			//verifies that all exchanges have occured
			var resultGroupings = results.GroupBy(r => r);
			foreach (var resultGrouping in resultGroupings)
			{
				Assert.That(resultGrouping.Count(), Is.EqualTo(num - 1));

			}
		}
	}

}
