using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.LINQ
{
	public class ReduceEnumerable : IEnumerable<Tuple<String, String>>
	{
		public ReduceEnumerable(IReducerRunner runner, IEnumerable<IGrouping<String,String>> mappedKeyValues)
		{
			_runner = runner;
		}

		private IReducerRunner _runner;
		private IEnumerable<IGrouping<String, String>> _mappedKeyValues;
		private IEnumerable<Tuple<String, String>> _results = null;

		public IEnumerator<Tuple<String, String>> GetEnumerator()
		{
			if (null == _results)
			{
				_results = _runner.RunReducers(_mappedKeyValues);
			}
			return _results.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
