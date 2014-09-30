using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapReduceMapper
	{
		void Map(String inputValue, IMapReduceContext context);
		//Task MapAsync(String inputValue, IMapReduceContext context);
	}
}
