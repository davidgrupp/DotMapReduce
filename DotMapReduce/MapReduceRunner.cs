using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce
{
	public class MapReduceRunner : IMapReduceRunner
	{
		public MapReduceRunner(IMapReduceMapper mapper, IMapReduceReducer reducer, IMapReduceFileService fileService)
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
			var context = new List<MapReduceContext>();
			context.Add(new MapReduceContext());

			//run the mappers
			List<String> docIds = _fileService.ReadDocumentIds(inputDirectory);
			foreach (var docId in docIds)
			{
				var inputValue = _fileService.ReadDocument(inputDirectory, docId);
				_mapper.Map(inputValue, context.First());
			}

			//run the reducers
			var keys = context.First().GetEmittedKeys();
			foreach (var key in keys)
			{
				var values = context.First().GetEmittedValues(key);
				_reducer.Reduce(key, values, context.First());
			}

			//save the results
			_fileService.CreateDirectory(outputDirectory);
			var docName = String.Format("Job{0:MM_dd_yy_hhmm}.txt", DateTime.Now);
			_fileService.CreateDocument(outputDirectory, docName);
			var aggergateKeys = context.First().GetAggergateKeys();
			foreach (var key in aggergateKeys)
			{
				var value = context.First().GetAggergateValue(key);
				_fileService.WriteToDocument(docName, String.Format("{0}: {1}", key, value));
			}
		}


	}
}
