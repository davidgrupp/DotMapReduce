using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces.Parallelization
{
	public interface IDataExchanger
	{
		void ExchangeData(List<IMapReduceWorker> workers);
	}
}
