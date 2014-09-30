using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce
{
	public class MapReduceContext : IMapReduceContext
	{
		private Dictionary<String, List<String>> _emittedValues = new Dictionary<String, List<String>>();
		private Dictionary<String, String> _emittedAggergateValues = new Dictionary<String, String>();

		public void Emit(String key, String value)
		{
			if (_emittedValues.ContainsKey(key))
			{
				_emittedValues[key].Add(value);
			}
			else
			{
				_emittedValues.Add(key, new List<String>());
				_emittedValues[key].Add(value);
			}
		}

		public void EmitKeyValue(String key, String value)
		{
			_emittedAggergateValues.Add(key, value);
		}

		public List<String> GetEmittedKeys()
		{
			return _emittedValues.Keys.ToList();
		}
		public List<String> GetEmittedValues(String key)
		{
			return _emittedValues[key];
		}

		public List<String> GetAggergateKeys()
		{
			return _emittedAggergateValues.Keys.ToList();
		}

		public String GetAggergateValue(String key)
		{
			return _emittedAggergateValues[key];
		}
	}
}
