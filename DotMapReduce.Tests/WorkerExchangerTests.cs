﻿using DotMapReduce.Interfaces.Parallelization;
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
	public class WorkerExchangerTests
	{
		[Test, Category("Unit")]
		public void GetExchanges()
		{
			//Arrange
			var num = 100;
			var totalExchanges = 0;
			object obj = "";
			var workers = Enumerable.Range(0, num).Select(i => new Mock<IMapReduceWorker>()).ToList();
			foreach (var worker in workers)
			{
				worker.Setup(w => w.Exchange(It.IsAny<IMapReduceWorker>()))
					.Callback(() => { lock (obj) { totalExchanges++; } });
			}

			//Act
			var exhngr = new WorkerExchanger();
			exhngr.ExchangeData(workers.Select(w => w.Object).ToList());

			//Assert
			var expected = (Math.Pow(num, 2) - num) / 2;
			Assert.That(totalExchanges, Is.EqualTo(expected));
		}
	}
}