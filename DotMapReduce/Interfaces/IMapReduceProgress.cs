using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapReduceProgress
	{
		IProgress<Int32> Mappers { get; set; }
		IProgress<Int32> Exchange { get; set; }
		IProgress<Int32> Reducers { get; set; }
		IProgress<Int32> Results { get; set; }
	}
}
