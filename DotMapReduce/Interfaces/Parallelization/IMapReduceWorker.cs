using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces.Parallelization
{
	public interface IMapReduceWorker
	{
		Int32 WorkerId { get; set; }
		IMapperContext MapperContext { get; set; }
		IReducerContext ReducerContext { get; set; }
		IMapReduceManager Manager { get; set; }

		Task RunMapperBatchAsync(String inputDirectory, List<String> idsBatch);
		Task RunReducersAsync(String outputDirectory, String outputFile);
		void SetReducerData(IEnumerable<IGrouping<String, String>> keyValueGroupings);
		void ExchangeKeyValues(IMapReduceWorker otherWorker);
	}
}
