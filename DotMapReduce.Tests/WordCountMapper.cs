using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests
{
	public class WordCountMapper : IMapReduceMapper
	{
		public void Map(string inputValue, IMapperContext context)
		{
			System.Threading.Thread.Sleep(100);
			var words = inputValue.Split(' ');
			foreach (var word in words)
			{
				context.Emit(word, "1");
			}
		}
	}
}
