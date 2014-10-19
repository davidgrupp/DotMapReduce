using DotMapReduce.Interfaces;
using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotMapReduce.Parallelization
{
	public class DataExchanger : IDataExchanger
	{
		private IEnumerable<IExchangable> _workers = null;

		public void ExchangeData(IEnumerable<IExchangable> workers)
		{
			_workers = workers;
			Exchange(0, _workers.Count() - 1).Wait();
		}

		private Task Exchange(Int32 start, Int32 end, IProgress<Int32> progress = null)
		{
			return Task.Run(() =>
			{
				if (start >= end)
					return;
				Int32 mid = ((start + end + 1) / 2);
				var rightListLen = end - mid + 1;

				var leftList = Enumerable.Range(start, mid - start).ToList();
				var rightList = Enumerable.Range(mid, rightListLen).ToList();

				for (var i = 0; i < rightList.Count() ; i++)
				{
					var exchangeTasks = new List<Task>();
					for (var k = 0; k < leftList.Count(); k++)
					{
						var rightElm = (i + k) % rightListLen;
						exchangeTasks.Add(DoExchange(leftList[k], rightList[rightElm]));
					}
					Task.WaitAll(exchangeTasks.ToArray());
				}

				var tskLeft = Exchange(start, mid - 1, progress);
				var tskRght = Exchange(mid, end, progress);

				Task.WaitAll(tskLeft, tskRght);

			});
		}

		private Task DoExchange(Int32 w1, Int32 w2)
		{
			return Task.Run(() =>
			{
				//Thread.Sleep(10);
				var worker1 = _workers.ElementAt(w1);
				var worker2 = _workers.ElementAt(w2);
				worker1.ExchangeKeyValues(worker2);
			});
		}


	}
}
