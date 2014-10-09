using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapperContext
	{
		Task EmitAsync(String key, String value);

		List<String> GetEmittedKeys();

		List<String> GetEmittedValues(String key);

		Dictionary<String, List<String>> GetPartitionedEmittedValues(Int32 partition);
	}
}
