using DotMapReduce.Interfaces;
using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Threaded
{
	public class ThreadedMapperRunner : IMapperRunner
	{
		public ThreadedMapperRunner(IMapper mapper, IMapReduceFileService fileService)
		{
			_mapper = mapper;
			_fileService = fileService;
		}

		private IMapper _mapper;
		private IMapReduceFileService _fileService;

		private List<IMapReduceWorker> _workers;
		private List<IMapReduceManager> _managers;

		public IEnumerable<IGrouping<String, String>> RunMappers()
		{
			_workers = new List<IMapReduceWorker>();
			_managers = new List<IMapReduceManager>();

			//get managers and workers
			//_managers.Add(new ThreadedMapReduceManager());
			var _manager = _managers.First();

			var numOfWorkers = 8;
			//_workers = Enumerable.Range(0, numOfWorkers).Select(i => new ThreadedMapReduceWorker(i, numOfWorkers, _manager, _fileService, _mapper, _reducer) as IMapReduceWorker).ToList();
			_manager.Workers.AddRange(_workers);

			//read the doc ids
			//List<String> docIds = _fileService.ReadDocumentIds(inputDirectory); // eventualy stream these

			//run the mappers
			//_manager.RunMappers(inputDirectory, docIds);

			throw new NotImplementedException();
		}
	}
}
