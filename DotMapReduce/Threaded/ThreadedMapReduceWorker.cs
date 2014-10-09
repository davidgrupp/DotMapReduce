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
		public ThreadedMapReduceWorker(Int32 workerId, Int32 totalWorkers, IMapReduceManager manager, IMapReduceFileService _fileService, IMapper _mapper)
			: this(workerId, totalWorkers, manager, _fileService, _mapper, new ThreadedMapperContext(totalWorkers), new ThreadedReducerContext())
		{

		}
		public ThreadedMapReduceWorker(Int32 workerId, Int32 totalWorkers, IMapReduceManager manager, IMapReduceFileService fileService, IMapper mapper, IMapperContext mapContext, IReducerContext rdcContext)
		{
			WorkerId = workerId;
			_totalWorkers = totalWorkers;
			MapperContext = mapContext;
			ReducerContext = rdcContext;
			_fileService = fileService;
			Manager = manager;
			_mapper = mapper;
			_workerKeyValues = new Dictionary<String, List<String>>();
		}
		public Int32 WorkerId { get; set; }
		public IMapperContext MapperContext { get; set; }
		public IReducerContext ReducerContext { get; set; }
		public IMapReduceManager Manager { get; set; }

		private IMapReduceFileService _fileService;
		private IMapper _mapper;
		private IReducer _reducer;
		private Int32 _totalWorkers;
		private Dictionary<String, List<String>> _workerKeyValues;

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

		public void RunMapperBatch(string inputDirectory, List<string> idsBatch)
		{
			throw new NotImplementedException();
		}

		private Task RunReducerBatch(String inputDirectory, List<String> keyBatch)
		{
			return Task.Run(() =>
			{
				Parallel.ForEach(_workerKeyValues.Keys, key =>
				{
					var values = _workerKeyValues[key];
					//throw if values is empty
					_reducer.Reduce(key, values, ReducerContext);
				});
			});
		}


		public void SetExchangeData(Dictionary<String, List<String>> keyValues)
		{
			foreach (var key in keyValues.Keys)
			{
				if (false == _workerKeyValues.ContainsKey(key))
					_workerKeyValues.Add(key, new List<String>());

				_workerKeyValues[key].AddRange(keyValues[key]);
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
