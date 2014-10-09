using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Generic
{
	public class GenericMapperContext : IMapperContext
	{
		private Dictionary<String, List<String>> _emittedValues = new Dictionary<String, List<String>>();
		public Task EmitAsync(String key, String value)
		{
			return Task.Run(() =>
			{
				lock (_emittedValues)
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
			});
		}

		public List<string> GetEmittedKeys()
		{
			return _emittedValues.Keys.ToList();
		}

		public List<string> GetEmittedValues(string key)
		{
			return _emittedValues[key];
		}

		public List<Tuple<string, string>> GetPartitionedEmittedValues(int partition)
		{
			throw new NotImplementedException();
		}
	}
}
