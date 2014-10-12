﻿using DotMapReduce.Interfaces;
using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Threaded
{
	public class ThreadedMapReduceRunner : IMapReduceRunner
	{
		public ThreadedMapReduceRunner(IMapper mapper, IReducer reducer, IMapReduceFileService fileService, IDataExchanger workerExchange)
		{
			_mapper = mapper;
			_reducer = reducer;
			_fileService = fileService;
			_workerExchange = workerExchange;
		}
		private IMapper _mapper;
		private IReducer _reducer;
		private IMapReduceFileService _fileService;
		private IDataExchanger _workerExchange;

		private List<IMapReduceWorker> _workers;
		private List<IMapReduceManager> _managers;

		public void Run(String inputDirectory, String outputDirectory)
		{
			_workers = new List<IMapReduceWorker>();
			_managers = new List<IMapReduceManager>();

			_managers.Add(new ThreadedMapReduceManager(_workerExchange));

			var _manager = _managers.First();

			var numOfWorkers = 8;
			_manager.Workers.AddRange(Enumerable.Range(0, numOfWorkers).Select(i => new ThreadedMapReduceWorker(i, numOfWorkers, _manager, _fileService, _mapper, _reducer)));

			//read the doc ids
			List<String> docIds = _fileService.ReadDocumentIds(inputDirectory); // eventual stream these

			//run the mappers
			_manager.RunMappers(inputDirectory, docIds);

			//exchange data between workers
			_manager.Exchange();

			//run the reducers
			//_manager.r

			////save the results
			//SaveReducerResults(outputDirectory);
		}





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

	}
}
