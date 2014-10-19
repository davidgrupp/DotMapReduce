using DotMapReduce.LINQ;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Tests.LINQ
{
	public class LINQMapReduceTests
	{
		[Test]
		public void RunMappers()
		{
			var values = new List<String>();
			values.Map((s, c) =>
			{
				var words = s.Split(' ');
				foreach (var word in words)
				{
					c.EmitAsync(word, "1");
				}
			});
		}

		[Test]
		public void RunReducers()
		{
			var values = new List<String>().GroupBy(s => s);
			values.Reduce((k, v, c) =>
			{
				c.EmitKeyValue(k, v.Count().ToString());
			});
		}

		[Test]
		public void RunMapReduce()
		{
			var values = new List<String>();
			values.Map((s, c) =>
			{
				var words = s.Split(' ');
				foreach (var word in words)
				{
					c.EmitAsync(word, "1");
				}
			})
			.Reduce((k, v, c) =>
			{
				c.EmitKeyValue(k, v.Count().ToString());
			});
		}
	}

	
}
