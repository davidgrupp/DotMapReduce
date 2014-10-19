using DotMapReduce.Interfaces;
using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Threaded
{
	public class ThreadedReducerRunner : IReducerRunner
	{
		public ThreadedReducerRunner(IReducer reducer, IMapReduceFileService fileService, IDataExchanger dataExchanger)
		{
			_reducer = reducer;
			_fileService = fileService;
			_dataExchanger = dataExchanger;
		}
		
		private IReducer _reducer;
		private IMapReduceFileService _fileService;
		private IDataExchanger _dataExchanger;

		public IEnumerable<Tuple<string, string>> RunReducers(IEnumerable<IGrouping<string, string>> mappedKeyValues)
		{
			throw new NotImplementedException();
		}
	}
}
