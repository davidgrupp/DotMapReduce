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
		private List<IMapReduceWorker> _workers = null;

		public void ExchangeData(List<IMapReduceWorker> workers)
		{
			_workers = workers;
			Exchange(0, _workers.Count - 1).Wait();
		}

		private Task Exchange(Int32 start, Int32 end, IProgress<Int32> progress = null)
		{
			return Task.Run(() =>
			{
				if (start >= end)
					return;
				Int32 mid = ((start + end + 1) / 2);
				for (var x = mid; x <= end; x++)
				{
					var exchangeTasks = new List<Task>();
					for (int y = start, i = 0; y <= mid - 1; y++, i++)
					{
						if ((x + i) <= end)
						{
							exchangeTasks.Add(DoExchange(y, x + i));
						}
						else
						{
							exchangeTasks.Add(DoExchange(y, x + i - mid));
						}
						if (null != progress)
							progress.Report(1);
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
				var worker1 = _workers[w1];
				var worker2 = _workers[w2];
				worker1.Exchange(worker2);
			});
		}


	}
}
