using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces.Parallelization
{
	public interface IMapReduceWorker
	{
		IMapReduceContext Context { get; set; }
		IMapReduceManager Manager { get; set; }

		//void RunMapperBatch(String inputDirectory, List<String> idsBatch);
		Task RunMapperBatchAsync(String inputDirectory, List<String> idsBatch);
	}
}
