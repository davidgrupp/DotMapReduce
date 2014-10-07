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
	[TestFixture]
	public class WorkerExchangerTests
	{
		[Test]
		public void GetExchanges()
		{
			var num = 100;
			var workers = Enumerable.Range(0, num).Select(i => new Mock<IMapReduceWorker>()).ToList();
			foreach (var worker in workers)
			{
				worker.Setup(w => w.Exchange(It.IsAny<IMapReduceWorker>()));
			}


			//
			var exhngr = new WorkerExchanger();
			exhngr.ExchangeData(workers.Select(w => w.Object).ToList());


			foreach (var worker in workers)
			{
				//worker.Verify(w => w.Exchange(It.IsAny<IMapReduceWorker>()), Times.Exactly(num - 1));
			}
		}
	}
}
