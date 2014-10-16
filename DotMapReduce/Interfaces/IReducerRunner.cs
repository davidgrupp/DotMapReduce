using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IReducerRunner
	{
		IEnumerable<Tuple<String,String>> RunReducers(IEnumerable<IGrouping<String,String>> mappedKeyValues);
	}
}
