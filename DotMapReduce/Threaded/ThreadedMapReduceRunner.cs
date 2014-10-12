using DotMapReduce.Interfaces;
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
		public ThreadedMapReduceRunner(IMapper mapper, IReducer reducer, IMapReduceFileService fileService, IDataExchanger workerdataExchange)
		{
			_mapper = mapper;
			_reducer = reducer;
			_fileService = fileService;
			_workerdataExchange = workerdataExchange;
		}
		private IMapper _mapper;
		private IReducer _reducer;
		private IMapReduceFileService _fileService;
		private IDataExchanger _workerdataExchange;

		private List<IMapReduceWorker> _workers;
		private List<IMapReduceManager> _managers;

		public void Run(String inputDirectory, String outputDirectory)
		{
			_workers = new List<IMapReduceWorker>();
			_managers = new List<IMapReduceManager>();

			//get managers and workers
			_managers.Add(new ThreadedMapReduceManager(_workerdataExchange));
			var _manager = _managers.First();

			var numOfWorkers = 8;
			_workers = Enumerable.Range(0, numOfWorkers).Select(i => new ThreadedMapReduceWorker(i, numOfWorkers, _manager, _fileService, _mapper, _reducer) as IMapReduceWorker).ToList();
			_manager.Workers.AddRange(_workers);

			//read the doc ids
			List<String> docIds = _fileService.ReadDocumentIds(inputDirectory); // eventualy stream these

			//run the mappers
			_manager.RunMappers(inputDirectory, docIds);

			//exchange data between workers
			_manager.Exchange();

			//run the reducers
			var docName = String.Format("Job{0:MM_dd_yy_hhmm}.txt", DateTime.Now);
			_manager.RunReducers(outputDirectory, docName);

		}





	}
}
