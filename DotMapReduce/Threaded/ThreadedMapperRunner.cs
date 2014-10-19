using DotMapReduce.Interfaces;
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

		public IEnumerable<IGrouping<String, String>> RunMappers()
		{
			throw new NotImplementedException();
		}
	}
}
