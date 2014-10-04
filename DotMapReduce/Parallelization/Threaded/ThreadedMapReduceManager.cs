using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Parallelization.Threaded
{
	public class ThreadedMapReduceManager : IMapReduceManager
	{
		public ThreadedMapReduceManager()
		{
			Workers = new List<IMapReduceWorker>();
		}
		public List<IMapReduceWorker> Workers { get; private set; }

		public void RunMappers(String inputDirectory, List<String> docIds)
		{
			var mapperTasks = new List<Task>();

			var docIdBatchSize = docIds.Count / Workers.Count;
			for (int docI = 0, wrkI = 0; wrkI < Workers.Count; docI += docIdBatchSize, wrkI++)
			{
				var task = Workers[wrkI].RunMapperBatchAsync(inputDirectory, docIds.Skip(docI).Take(docIdBatchSize).ToList());
				mapperTasks.Add(task);
			}

			Task.WaitAll(mapperTasks.ToArray());
		}

		public void RunReducers()
		{

		}

		public void SendPartitionedData()
		{

		}

		public void SaveResults()
		{

		}
	}
}
