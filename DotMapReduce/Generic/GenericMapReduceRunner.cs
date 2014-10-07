using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Generic
{
	public class GenericMapReduceRunner : IMapReduceRunner
	{
		public GenericMapReduceRunner(IMapReduceMapper mapper, IMapReduceReducer reducer, IMapReduceFileService fileService)
		{
			_mapper = mapper;
			_reducer = reducer;
			_fileService = fileService;
		}
		private IMapReduceMapper _mapper;
		private IMapReduceReducer _reducer;
		private IMapReduceFileService _fileService;

		public void Run(String inputDirectory, String outputDirectory)
		{
			var mapContext = new GenericMapperContext();
			var rdcContext = new GenericReducerContext();

			//run the mappers
			List<String> docIds = _fileService.ReadDocumentIds(inputDirectory);
			foreach (var docId in docIds)
			{
				var inputValue = _fileService.ReadDocument(inputDirectory, docId);
				_mapper.Map(inputValue, mapContext);
			}

			//run the reducers
			var keys = mapContext.GetEmittedKeys();
			foreach (var key in keys)
			{
				var values = mapContext.GetEmittedValues(key);
				_reducer.Reduce(key, values, rdcContext);
			}

			//save the results
			_fileService.CreateDirectory(outputDirectory);
			var docName = String.Format("Job{0:MM_dd_yy_hhmm}.txt", DateTime.Now);
			_fileService.CreateDocument(outputDirectory, docName);
			var aggergateKeys = rdcContext.GetKeys();
			foreach (var key in aggergateKeys)
			{
				var value = rdcContext.GetValue(key);
				_fileService.WriteToDocument(docName, String.Format("{0}: {1}", key, value));
			}
		}


	}
}
