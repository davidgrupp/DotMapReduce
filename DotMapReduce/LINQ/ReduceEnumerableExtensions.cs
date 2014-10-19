using DotMapReduce.Interfaces;
using DotMapReduce.Parallelization;
using DotMapReduce.Threaded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.LINQ
{
	public static class ReduceEnumerableExtensions
	{
		public static IEnumerable<Tuple<String, String>> Reduce(this IEnumerable<IGrouping<String, String>> values, Action<String, IEnumerable<String>, IReducerContext> reduceFunc)
		{
			var reducer = new LinqMapperReducer(reduceFunc);
			var memoryFileService = new MemoryFileSerivce();
			var exchanger = new DataExchanger();
			if (values is MapEnumerable)
			{

			}
			var runner = new ThreadedReducerRunner(reducer, memoryFileService, exchanger);
			var results = new ReduceEnumerable(runner, values);
			return results;

		}
	}
}
