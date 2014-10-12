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
		protected Dictionary<String, String> _emittedAggergateValues = new Dictionary<String, String>();
		
		public virtual void EmitKeyValue(String key, String value)
		{
			lock (_emittedAggergateValues)
			{
				_emittedAggergateValues.Add(key, value);
			}
		}

		public virtual List<String> GetKeys()
		{
			return _emittedAggergateValues.Keys.ToList();
		}

		public virtual String GetValue(String key)
		{
			return _emittedAggergateValues[key];
		}


		public virtual string GetNextKey()
		{
			throw new NotImplementedException();
		}
	}
}
