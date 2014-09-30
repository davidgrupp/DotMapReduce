using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapReduceRunner
	{
		void Run(String inputDirectory, String outputDirectory);
	}
}
