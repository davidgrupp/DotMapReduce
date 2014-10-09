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
			_mapPartitions = new Dictionary<Int32, SortedDictionary<String, IMapGrouping<String, String>>>();
		}
		private Int32 _totalWorkers;
		private Dictionary<Int32, SortedDictionary<String, IMapGrouping<String, String>>> _mapPartitions;

		public Task EmitAsync(String key, String value)
		{
			return Task.Run(() =>
			{
				var partition = PartitionUtilities.GetPartition(key, _totalWorkers);

				lock (_mapPartitions)
				{
					if (false == _mapPartitions.ContainsKey(partition))
						_mapPartitions.Add(partition, new SortedDictionary<String, IMapGrouping<String, String>>());

					if (false == _mapPartitions[partition].ContainsKey(key))
						_mapPartitions[partition].Add(key, new MapGrouping<String, String>(key, new List<String>()));

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

		public IEnumerable<IGrouping<String, String>> GetPartitionedEmittedValues(Int32 partition)
		{
			if (_mapPartitions.ContainsKey(partition))
				return _mapPartitions[partition].Values;
			else
				return new List<IGrouping<String, String>>();
		}

	}
}
