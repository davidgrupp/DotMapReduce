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
			_mapPartitions = new Dictionary<Int32, Dictionary<String, List<String>>>();
		}
		private Int32 _totalWorkers;
		private Dictionary<Int32, Dictionary<String, List<String>>> _mapPartitions;

		public Task EmitAsync(String key, String value)
		{
			return Task.Run(() =>
			{
				var partition = PartitionUtilities.GetPartition(key, _totalWorkers);

				lock (_mapPartitions)
				{
					if (false == _mapPartitions.ContainsKey(partition))
						_mapPartitions.Add(partition, new Dictionary<String, List<String>>());

					if (false == _mapPartitions[partition].ContainsKey(key))
						_mapPartitions[partition].Add(key, new List<String>());

					_mapPartitions[partition][key].Add(value);
				}
			});
		}

		public List<String> GetEmittedKeys()
		{
			throw new NotImplementedException();
		}

		public List<String> GetEmittedValues(String key)
		{
			throw new NotImplementedException();
		}

		public Dictionary<String, List<String>> GetPartitionedEmittedValues(Int32 partition)
		{
			if (_mapPartitions.ContainsKey(partition))
				return _mapPartitions[partition];
			else
				return new Dictionary<String, List<String>>();
		}

	}
}
