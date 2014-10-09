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
	public class ThreadedMapperContext : IMapperContext
	{
		public ThreadedMapperContext(Int32 totalWorkers)
		{
			_totalWorkers = totalWorkers;
			_mapPartitions = new Dictionary<Int32, List<Tuple<String, String>>>();
		}
		private Int32 _totalWorkers;
		private Dictionary<Int32, List<Tuple<String, String>>> _mapPartitions;
		public void Emit(String key, String value)
		{
			var partition = PartitionUtilities.GetPartition(key, _totalWorkers);
			if (_mapPartitions.ContainsKey(partition))
			{
				_mapPartitions[partition].Add(new Tuple<String, String>(key, value));
			}
			else
			{
				_mapPartitions.Add(partition, new List<Tuple<String, String>>());
				_mapPartitions[partition].Add(new Tuple<String, String>(key, value));
			}
		}

		public List<String> GetEmittedKeys()
		{
			throw new NotImplementedException();
		}

		public List<String> GetEmittedValues(String key)
		{
			throw new NotImplementedException();
		}

		public List<Tuple<String, String>> GetPartitionedEmittedValues(Int32 partition)
		{
			return _mapPartitions[partition];
		}
		
	}
}
