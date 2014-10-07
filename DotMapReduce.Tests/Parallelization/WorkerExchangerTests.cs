using DotMapReduce.Parallelization;
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

			//
			var exhngr = new WorkerExchanger();
			var results = exhngr.GetExchanges();
		}
	}
}
