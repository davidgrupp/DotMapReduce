using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests
{
	public class WordCountReducer : IMapReduceReducer
	{
		public void Reduce(String key, IEnumerable<String> values, IMapReduceContext context)
		{
			context.EmitKeyValue(key, values.Count().ToString());
		}
	}
}
