using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces.Parallelization
{
	public interface IMapReduceManager
	{
		List<IMapReduceWorker> Workers { get; }
		void RunMappers(String inputDirectory, List<String> docIds);
		void Exchange();
		void RunReducers(String outputDirectory, String outputFile);
	}
}
