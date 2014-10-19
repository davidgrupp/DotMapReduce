using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.LINQ
{
	public class LinqMapperReducer : IMapper, IReducer
	{
		public LinqMapperReducer(Action<String, IMapperContext> map)
		{
			_map = map;
		}

		public LinqMapperReducer(Action<String, IEnumerable<String>, IReducerContext> reduce)
		{
			_reduce = reduce;
		}

		private Action<String, IMapperContext> _map;
		private Action<String, IEnumerable<String>, IReducerContext> _reduce;

		public void Map(string inputValue, IMapperContext context)
		{
			_map(inputValue, context);
		}

		public void Reduce(string key, IEnumerable<string> values, IReducerContext context)
		{
			_reduce(key, values, context);
		}
	}
}
