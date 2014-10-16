using DotMapReduce.Interfaces;
using DotMapReduce.Parallelization;
using DotMapReduce.Threaded;
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

	public static class Extens
	{
		public static IQueryable<IGrouping<String, String>> Map(this IEnumerable<String> values, Action<String, IMapperContext> mapperFunc)
		{
			var mapper = new LinqMapperReducer(mapperFunc);
			var memoryFileService = new Mock<IMapReduceFileService>();
			var runner = new ThreadedMapReduceRunner(mapper, null, memoryFileService.Object, null);
			runner.Run("", "");
			return null;
		}

		public static IEnumerable<Tuple<String, String>> Reduce(this IEnumerable<IGrouping<String, String>> values, Action<String, IEnumerable<String>, IReducerContext> reduceFunc)
		{
			var mapper = new LinqMapperReducer(reduceFunc);
			var memoryFileService = new Mock<IMapReduceFileService>();
			var exchanger = new DataExchanger();
			var runner = new ThreadedMapReduceRunner(mapper, null, memoryFileService.Object, exchanger);
			runner.Run("", "");
			return null;
		}

		private class MapKeyValuesCollection : IQueryable<IGrouping<String, String>>
		{
			private IMapperRunner _mapperRunner;
			private IEnumerable<IGrouping<String, String>> _mapperResults;

			public IEnumerator<IGrouping<string, string>> GetEnumerator()
			{
				_mapperRunner.RunMappers();


				throw new NotImplementedException();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			private class MsfIEnumerator : IEnumerator<String>
			{

				public string Current
				{
					get { throw new NotImplementedException(); }
				}

				public void Dispose()
				{
					throw new NotImplementedException();
				}

				object System.Collections.IEnumerator.Current
				{
					get { return Current; }
				}

				public bool MoveNext()
				{
					throw new NotImplementedException();
				}

				public void Reset()
				{
					throw new NotImplementedException();
				}


			}

			public Type ElementType
			{
				get { throw new NotImplementedException(); }
			}

			public System.Linq.Expressions.Expression Expression
			{
				get { throw new NotImplementedException(); }
			}

			public IQueryProvider Provider
			{
				get { throw new NotImplementedException(); }
			}
		}

		private class LinqMapperReducer : IMapper, IReducer
		{
			public LinqMapperReducer(Action<String, IMapperContext> map)
			{
				_map = map;
			}

			public LinqMapperReducer(Action<String, IEnumerable<String>, IReducerContext> reduce)
			{
				_reduce = reduce;
			}

			private Action<String, IMapperContext> _map;
			private Action<String, IEnumerable<String>, IReducerContext> _reduce;

			public void Map(string inputValue, IMapperContext context)
			{
				_map(inputValue, context);
			}

			public void Reduce(string key, IEnumerable<string> values, IReducerContext context)
			{
				_reduce(key, values, context);
			}
		}
	}
}
