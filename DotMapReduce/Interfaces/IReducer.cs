using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IReducer
	{
		void Reduce(String key, IEnumerable<String> values, IReducerContext context);
	}
}
