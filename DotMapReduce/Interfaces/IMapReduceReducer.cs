using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapReduceReducer
	{
		void Reduce(String key, IEnumerable<String> values, IMapReduceContext context);
		//Task ReduceAsync(String key, IEnumerable<String> values, IMapReduceContext context);
	}
}
