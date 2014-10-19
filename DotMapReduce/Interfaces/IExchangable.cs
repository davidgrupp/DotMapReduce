using DotMapReduce.Interfaces.Parallelization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IExchangable
	{
		void ExchangeKeyValues(IExchangable otherExchangable);
	}
}
