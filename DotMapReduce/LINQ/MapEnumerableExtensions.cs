using DotMapReduce.Interfaces;
using DotMapReduce.Threaded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.LINQ
{
	public static class MapEnumerableExtensions
	{
		public static IEnumerable<IGrouping<String, String>> Map(this IEnumerable<String> values, Action<String, IMapperContext> mapperFunc)
		{
			var mapper = new LinqMapperReducer(mapperFunc);
			var memoryFileService = new MemoryFileSerivce(values);
			var runner = new ThreadedMapperRunner(mapper, memoryFileService);
			var results = new MapEnumerable(runner);
			return results;
		}
	}
}
