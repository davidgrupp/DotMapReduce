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
			_mapPartitions = new SortedDictionary<String, IMapGrouping<String, String>>[totalWorkers];
			for (var i = 0; i < totalWorkers; i++)
			{
				_mapPartitions[i] = new SortedDictionary<String, IMapGrouping<String, String>>();
			}
		}
		private Int32 _totalWorkers;
		private SortedDictionary<String, IMapGrouping<String, String>>[] _mapPartitions;

		public Task EmitAsync(String key, String value)
		{
			return Task.Run(() =>
			{
				var partition = PartitionUtilities.GetPartition(key, _totalWorkers);

				SortedDictionary<String, IMapGrouping<String, String>> partitionKeyValues;

				lock (_mapPartitions)
				{
					partitionKeyValues = _mapPartitions[partition];
				}

				lock (partitionKeyValues)
				{
					if (false == partitionKeyValues.ContainsKey(key))
						partitionKeyValues.Add(key, new MapGrouping<String, String>(key, new List<String>()));

					partitionKeyValues[key].Add(value);
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
			if (_mapPartitions.Count() > partition)
				return _mapPartitions[partition].Values;
			else
				return new List<IGrouping<String, String>>();
		}

	}
}
