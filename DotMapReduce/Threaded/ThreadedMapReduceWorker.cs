﻿using DotMapReduce.Interfaces;
using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Threaded
{
	public class ThreadedMapReduceWorker : IMapReduceWorker
	{
		public ThreadedMapReduceWorker(Int32 workerId, Int32 totalWorkers, IMapReduceManager manager, IMapReduceFileService fileService, IMapper mapper, IReducer reducer)
			: this(workerId, totalWorkers, manager, fileService, mapper, reducer, new ThreadedMapperContext(totalWorkers), new ThreadedReducerContext())
		{

		}
		public ThreadedMapReduceWorker(Int32 workerId, Int32 totalWorkers, IMapReduceManager manager, IMapReduceFileService fileService, IMapper mapper, IReducer reducer, IMapperContext mapContext, IReducerContext rdcContext)
		{
			WorkerId = workerId;
			_totalWorkers = totalWorkers;
			MapperContext = mapContext;
			ReducerContext = rdcContext;
			_fileService = fileService;
			Manager = manager;
			_mapper = mapper;
			_reducer = reducer;
			_workerKeyValues = new List<IMapGrouping<String, String>>();
		}
		public Int32 WorkerId { get; set; }
		public IMapperContext MapperContext { get; set; }
		public IReducerContext ReducerContext { get; set; }
		public IMapReduceManager Manager { get; set; }

		private IMapReduceFileService _fileService;
		private IMapper _mapper;
		private IReducer _reducer;
		private Int32 _totalWorkers;
		private List<IMapGrouping<String, String>> _workerKeyValues;

		public Task RunMapperBatchAsync(String inputDirectory, List<String> idsBatch)
		{
			return Task.Run(() =>
			{
				Parallel.ForEach(idsBatch, docId =>
				{
					var inputValue = _fileService.ReadDocument(inputDirectory, docId);
					_mapper.Map(inputValue, MapperContext);
				});

				SetReducerData(MapperContext.GetPartitionedEmittedValues(this.WorkerId));
			});
		}

		public Task RunReducersAsync(String outputDirectory, String outputFile)
		{
			return Task.Run(() =>
			{
				Parallel.ForEach(_workerKeyValues, grouping =>
				{
					//TODO:throw if values is empty?
					_reducer.Reduce(grouping.Key, grouping, ReducerContext);
					//save last results
					SaveLastReducerResult(outputDirectory, outputFile);
				});
				SaveRemainingReducerResults(outputDirectory, outputFile);
			});
		}

		public void SetReducerData(IEnumerable<IGrouping<String, String>> keyValueGroupings)
		{
			foreach (var grouping in keyValueGroupings)
			{
				if (false == _workerKeyValues.Any(g => g.Key == grouping.Key))
					_workerKeyValues.Add(new MapGrouping<String, String>(grouping.Key, new List<String>()));

				if (grouping.Any())
					_workerKeyValues.First(g => g.Key == grouping.Key).AddRange(grouping);
			}
		}

		public void ExchangeKeyValues(IExchangable otherExchangable)
		{
			if (otherExchangable is IMapReduceWorker)
			{
				var otherWorker = otherExchangable as IMapReduceWorker;
				var _otherWorkersData = MapperContext.GetPartitionedEmittedValues(otherWorker.WorkerId);
				otherWorker.SetReducerData(_otherWorkersData);

				var currentWorkersData = otherWorker.MapperContext.GetPartitionedEmittedValues(this.WorkerId);
				this.SetReducerData(currentWorkersData);
			}
		}

		private void SaveLastReducerResult(String outputDirectory, String outputFile)
		{
			var nextKey = ReducerContext.GetNextKey();
			if (null != nextKey)
			{
				var value = ReducerContext.GetValue(nextKey);
				_fileService.WriteToDocument(outputFile, String.Format("{0}: {1}", nextKey, value));
			}
		}

		private void SaveRemainingReducerResults(String outputDirectory, String outputFile)
		{
			String nextKey;
			if (null != (nextKey = ReducerContext.GetNextKey()))
			{
				var value = ReducerContext.GetValue(nextKey);
				_fileService.WriteToDocument(outputFile, String.Format("{0}: {1}", nextKey, value));
			}
		}
	}
}
