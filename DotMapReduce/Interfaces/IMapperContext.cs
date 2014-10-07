using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapperContext
	{
		void Emit(String key, String value);

		List<String> GetEmittedKeys();

		List<String> GetEmittedValues(String key);

		List<Tuple<String, String>> GetPartitionedEmittedValues(Int32 partition);
	}
}
