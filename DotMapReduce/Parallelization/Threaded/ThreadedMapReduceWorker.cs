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
		public ThreadedMapReduceWorker(IMapReduceFileService _fileService, IMapReduceMapper _mapper)
			: this(_fileService, _mapper, new MapReduceContext())
		{

		}
		public ThreadedMapReduceWorker(IMapReduceFileService fileService, IMapReduceMapper mapper, IMapReduceContext context)
		{
			Context = context;
			_fileService = fileService;
			_mapper = mapper;
		}
		public IMapReduceContext Context { get; set; }
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
					_mapper.Map(inputValue, Context);
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





	}
}
