using DotMapReduce.Interfaces;
using DotMapReduce.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Threaded
{
	public class ThreadedReducerContext : Generic.GenericReducerContext
	{
		private Queue<String> _emittedKeys = new Queue<String>();

		public override void EmitKeyValue(String key, String value)
		{
			base.EmitKeyValue(key, value);
			_emittedKeys.Enqueue(key);
		}

		public override String GetNextKey()
		{
			lock (_emittedKeys)
			{
				if (_emittedKeys.Any())
					return _emittedKeys.Dequeue();
				else
					return null;
			}
		}
	}
}
