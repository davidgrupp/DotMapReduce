using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce
{
	public class ParallelMapReduceRunner : IMapReduceRunner
	{
		public ParallelMapReduceRunner(IMapReduceMapper mapper, IMapReduceReducer reducer, IMapReduceFileService fileService)
		{
			_mapper = mapper;
			_reducer = reducer;
			_fileService = fileService;
		}
		private IMapReduceMapper _mapper;
		private IMapReduceReducer _reducer;
		private IMapReduceFileService _fileService;

		private List<IMapReduceContext> _mapperContexts;
		private List<IMapReduceContext> _reducerContexts;

		public void Run(String inputDirectory, String outputDirectory)
		{
			_mapperContexts = new List<IMapReduceContext>();
			_reducerContexts = new List<IMapReduceContext>();

			//read the doc ids
			List<String> docIds = _fileService.ReadDocumentIds(inputDirectory); // eventual stream these

			//run the mappers
			List<String> docIdBatch = new List<String>();
			var docIdBatchSize = Math.Min(Math.Log(docIds.Count), 100);
			for (var i = 0; i < docIds.Count; i++)
			{
				docIdBatch.Add(docIds[i]);
				if (docIdBatch.Count >= docIdBatchSize || i + 1 == docIds.Count)
				{
					RunMapperBatch(inputDirectory, docIdBatch.ToList());
					docIdBatch.Clear();
				}
			}

			//run the reducers
			var keys = CombineKeys(_mapperContexts);
			var keyBatch = new List<String>();
			var keyBatchSize = Math.Min(Math.Log(keys.Count), 100);
			for (var i = 0; i < keys.Count; i++)
			{
				keyBatch.Add(keys[i]);
				if (keys.Count >= keyBatchSize || i + 1 == keys.Count)
				{
					RunReducerBatch(inputDirectory, new List<String>());
				}
			}

			//save the results
			SaveReducerResults(outputDirectory);
		}

		private void RunMapperBatch(String inputDirectory, List<String> idsBatch)
		{
			var context = new MapReduceContext();
			_mapperContexts.Add(context);

			foreach (var docId in idsBatch)
			{
				var inputValue = _fileService.ReadDocument(inputDirectory, docId);
				_mapper.Map(inputValue, context);
			}
		}

		private void RunReducerBatch(String inputDirectory, List<String> keyBatch)
		{
			var reducerContext = new MapReduceContext();
			_reducerContexts.Add(reducerContext);

			foreach (var key in keyBatch)
			{
				var allValues = new List<String>();
				foreach (var context in _mapperContexts)
				{
					allValues.AddRange(context.GetEmittedValues(key));
				}
				_reducer.Reduce(key, allValues, reducerContext);
			}
		}

		private void SaveReducerResults(String outputDirectory)
		{
			_fileService.CreateDirectory(outputDirectory);
			var docName = String.Format("Job{0:MM_dd_yy_hhmm}.txt", DateTime.Now);
			_fileService.CreateDocument(outputDirectory, docName);
			foreach (var context in _reducerContexts)
			{
				var contextKeys = context.GetAggergateKeys();
				foreach (var key in contextKeys)
				{
					var value = context.GetAggergateValue(key);
					_fileService.WriteToDocument(docName, String.Format("{0}: {1}", key, value));
				}
			}
		}

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
