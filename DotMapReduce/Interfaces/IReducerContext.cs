using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IReducerContext
	{
		void EmitKeyValue(String key, String value);
		
		List<String> GetKeys();

		String GetValue(String key);

		String GetNextKey();
	}
}
