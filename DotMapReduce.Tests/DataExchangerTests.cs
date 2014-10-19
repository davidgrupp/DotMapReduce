using DotMapReduce.Interfaces;
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
		[TestCase(2, 10)]
		[TestCase(99, 101)]
		public void GetExchanges(Int32 startnum, Int32 endnum)
		{
			for (var num = startnum; num <= endnum; num++)
			{
				Console.WriteLine(num);
				//Arrange
				object obj = "";
				var workers = Enumerable.Range(0, num).Select(i => new Mock<IMapReduceWorker>()).ToList();
				var indx = 0;
				var results = new List<Int32>();
				foreach (var worker in workers)
				{
					var w1 = indx++;
					worker.SetupGet(w => w.WorkerId).Returns(w1);
					worker.Setup(w => w.ExchangeKeyValues(It.IsAny<IExchangable>()))
						.Callback<IExchangable>((w2) =>
						{
							lock (results) { results.Add(w1); results.Add(((IMapReduceWorker)w2).WorkerId); }
						});
				}

				//Act
				var exhngr = new DataExchanger();
				exhngr.ExchangeData(workers.Select(w => w.Object).ToList());

				//Assert
				//verifies that the total number of exchanges is correct
				var expected = Math.Pow(num, 2) - num;
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

}
