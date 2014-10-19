using DotMapReduce.Interfaces;
using DotMapReduce.Threaded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.LINQ
{
	class MapEnumerable : IEnumerable<IGrouping<String, String>>
	{
		public MapEnumerable(IMapperRunner runner)
		{
			_runner = runner;
		}

		private IMapperRunner _runner;
		private IEnumerable<IGrouping<String, String>> _results = null;
		
		public IEnumerator<IGrouping<String, String>> GetEnumerator()
		{
			if (null == _results)
			{
				_results = _runner.RunMappers();
			}
			return _results.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
