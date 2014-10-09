using DotMapReduce.Interfaces;
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
		//private Dictionary<String, List<String>> _workerKeyValues;
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
			});
		}

		public Task RunReducersAsync()
		{
			return Task.Run(() =>
			{
				Parallel.ForEach(_workerKeyValues, grouping =>
				{
					//TODO:throw if values is empty?
					_reducer.Reduce(grouping.Key, grouping, ReducerContext);
				});
			});
		}


		public void SetExchangeData(IEnumerable<IGrouping<String, String>> keyValueGroupings)
		{
			foreach (var grouping in keyValueGroupings)
			{
				if (false == _workerKeyValues.Any(g => g.Key == grouping.Key))
					_workerKeyValues.Add(new MapGrouping<String, String>(grouping.Key, new List<String>()));

				_workerKeyValues.First(g => g.Key == grouping.Key).AddRange(grouping);
			}
		}

		public void ExchangeKeyValues(IMapReduceWorker otherWorker)
		{
			var _otherWorkersData = MapperContext.GetPartitionedEmittedValues(otherWorker.WorkerId);
			otherWorker.SetExchangeData(_otherWorkersData);

			var currentWorkersData = otherWorker.MapperContext.GetPartitionedEmittedValues(this.WorkerId);
			this.SetExchangeData(currentWorkersData);
		}
	}
}
