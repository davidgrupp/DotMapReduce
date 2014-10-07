using DotMapReduce.Interfaces;
using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Parallelization.Threaded
{
	public class ThreadedMapReduceWorker : IMapReduceWorker
	{
		public ThreadedMapReduceWorker(Int32 workerId, Int32 totalWorkers, IMapReduceManager manager, IMapReduceFileService _fileService, IMapReduceMapper _mapper)
			: this(workerId, manager, _fileService, _mapper, new ThreadedMapperContext(totalWorkers), new ThreadedReducerContext())
		{

		}
		public ThreadedMapReduceWorker(Int32 workerId, IMapReduceManager manager, IMapReduceFileService fileService, IMapReduceMapper mapper, IMapperContext mapContext, IReducerContext rdcContext)
		{
			WorkerId = workerId;
			MapperContext = mapContext;
			ReducerContext = rdcContext;
			_fileService = fileService;
			Manager = manager;
			_mapper = mapper;
		}
		public Int32 WorkerId { get; set; }
		public IMapperContext MapperContext { get; set; }
		public IReducerContext ReducerContext { get; set; }
		public IMapReduceManager Manager { get; set; }
		private IMapReduceFileService _fileService;
		private IMapReduceMapper _mapper;


		public Task RunMapperBatchAsync(String inputDirectory, List<String> idsBatch)
		{
			return Task.Run(() =>
			{
				foreach (var docId in idsBatch)
				{
					var inputValue = _fileService.ReadDocument(inputDirectory, docId);
					_mapper.Map(inputValue, MapperContext);
				}
			});
		}

		public void RunMapperBatch(string inputDirectory, List<string> idsBatch)
		{
			throw new NotImplementedException();
		}

		private void RunReducerBatch(String inputDirectory, List<String> keyBatch)
		{
		}


		public void Exchange(IMapReduceWorker worker)
		{
			
		}

		
		public List<String> ExchangeKeyValues(List<String> values, IMapReduceWorker otherWorker)
		{
			var otherWorkersData = new List<String>(); //otherWorker.WorkerId
			var _otherWorkersData = MapperContext.GetPartitionedEmittedValues(otherWorker.WorkerId);

			var myData = new List<String>();
			var currentWorkersData = otherWorker.MapperContext.GetPartitionedEmittedValues(this.WorkerId);
			myData.AddRange(values);

			return otherWorkersData;
		}
	}
}
