using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Parallelization
{
	public class WorkerExchanger
	{
		public List<Tuple<Int32, Int32>> GetExchanges()
		{
			var results = new List<Tuple<Int32, Int32>>();

			var num = 10;
			var vals = new int[num, num - 1];
			var tmp = Enumerable.Range(0, num - 1).Select(n => Enumerable.Range(n + 1, (num - n) - 1).ToList()).ToArray();

			var current = new List<Int32>();
			for (var x = 0; x < num - 1; x++)
			{
				current = new List<Int32>();
				var first = tmp[0].First();
				tmp[0].Remove(first);
				current.Add(0);
				current.Add(first);
				results.Add(new Tuple<Int32, Int32>(0, first));
				for (var y = 1; y < num - 1; y++)
				{
					if (false == current.Any(v => v == y) && tmp[y].Any())
					{
						var next = tmp[y].FirstOrDefault(v => false == current.Any(w => w == v));
						if (0 < next)
						{
							current.Add(y);
							tmp[y].Remove(next);

							current.Add(next);
							results.Add(new Tuple<Int32, Int32>(y, next));
						}
					}
				}
			}

			return results;
		}
	}
}
