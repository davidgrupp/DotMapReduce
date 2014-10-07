using DotMapReduce.Interfaces;
using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Parallelization.Threaded
{
	public class ThreadedMapReduceRunner : IMapReduceRunner
	{
		public ThreadedMapReduceRunner(IMapReduceMapper mapper, IMapReduceReducer reducer, IMapReduceFileService fileService, IWorkerExchanger workerExchange)
		{
			_mapper = mapper;
			_reducer = reducer;
			_fileService = fileService;
			_workerExchange = workerExchange;
		}
		private IMapReduceMapper _mapper;
		private IMapReduceReducer _reducer;
		private IMapReduceFileService _fileService;
		private IWorkerExchanger _workerExchange;

		private List<IMapReduceWorker> _workers;
		private List<IMapReduceManager> _managers;

		public void Run(String inputDirectory, String outputDirectory)
		{
			_workers = new List<IMapReduceWorker>();
			_managers = new List<IMapReduceManager>();

			_managers.Add(new ThreadedMapReduceManager(_workerExchange));

			var _manager = _managers.First();

			var numOfWorkers = 8;
			_manager.Workers.AddRange(Enumerable.Range(0, numOfWorkers).Select(i => new ThreadedMapReduceWorker(i, _fileService, _mapper)));

			//read the doc ids
			List<String> docIds = _fileService.ReadDocumentIds(inputDirectory); // eventual stream these

			//run the mappers
			_manager.RunMappers(inputDirectory, docIds);

			//exchange data between works
			

			//run the reducers
			//var keys = CombineKeys(_mapperContexts);
			//var keyBatch = new List<String>();
			//var keyBatchSize = Math.Min(Math.Log(keys.Count), 100);
			//for (var i = 0; i < keys.Count; i++)
			//{
			//	keyBatch.Add(keys[i]);
			//	if (keys.Count >= keyBatchSize || i + 1 == keys.Count)
			//	{
			//		RunReducerBatch(inputDirectory, new List<String>());
			//	}
			//}

			////save the results
			//SaveReducerResults(outputDirectory);
		}

		

		//private void RunReducerBatch(String inputDirectory, List<String> keyBatch)
		//{
		//	var reducerContext = new MapReduceContext();
		//	_reducerContexts.Add(reducerContext);

		//	foreach (var key in keyBatch)
		//	{
		//		var allValues = new List<String>();
		//		foreach (var context in _mapperContexts)
		//		{
		//			allValues.AddRange(context.GetEmittedValues(key));
		//		}
		//		_reducer.Reduce(key, allValues, reducerContext);
		//	}
		//}

		//private void SaveReducerResults(String outputDirectory)
		//{
		//	_fileService.CreateDirectory(outputDirectory);
		//	var docName = String.Format("Job{0:MM_dd_yy_hhmm}.txt", DateTime.Now);
		//	_fileService.CreateDocument(outputDirectory, docName);
		//	foreach (var context in _reducerContexts)
		//	{
		//		var contextKeys = context.GetAggergateKeys();
		//		foreach (var key in contextKeys)
		//		{
		//			var value = context.GetAggergateValue(key);
		//			_fileService.WriteToDocument(docName, String.Format("{0}: {1}", key, value));
		//		}
		//	}
		//}

		private List<String> CombineKeys(List<IMapReduceContext> contexts)
		{
			List<String> allKeys = new List<String>();
			foreach (var context in contexts)
			{
				allKeys.AddRange(context.GetEmittedKeys());
			}

			return allKeys.Distinct().ToList();
		}
	}
}
