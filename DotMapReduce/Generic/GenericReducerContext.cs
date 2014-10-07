using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Generic
{
	public class GenericReducerContext : IReducerContext
	{
		private Dictionary<String, String> _emittedAggergateValues = new Dictionary<String, String>();
		public void EmitKeyValue(String key, String value)
		{
			_emittedAggergateValues.Add(key, value);
		}

		public List<String> GetKeys()
		{
			return _emittedAggergateValues.Keys.ToList();
		}

		public String GetValue(String key)
		{
			return _emittedAggergateValues[key];
		}
	}
}
