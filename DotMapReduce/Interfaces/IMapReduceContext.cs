using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapReduceContext
	{
		void Emit(String key, String value);

		void EmitKeyValue(String key, String value);
		//Task EmitAsync(String key, String value);

		//Task<List<String>> GetEmittedKeysAsync();
		//Task<List<String>> GetEmittedValuesAsync(String key);
		
		List<String> GetEmittedKeys();
		List<String> GetEmittedValues(String key);
	}
}
