using DotMapReduce.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce
{
	public class MapGrouping<TKey, TElement> : IMapGrouping<TKey, TElement>
	{
		public MapGrouping(IGrouping<TKey, TElement> grouping)
		{
			if (grouping == null)
				throw new ArgumentNullException("grouping");
			Key = grouping.Key;
			_elements = grouping.ToList();
		}

		public MapGrouping(TKey key, List<TElement> elements)
		{
			if (key == null)
				throw new ArgumentNullException("grouping");
			Key = key;
			_elements = elements;
		}

		public TKey Key { get; private set; }

		private readonly List<TElement> _elements;

		public IEnumerator<TElement> GetEnumerator()
		{
			return this._elements.GetEnumerator();
		}


		public void Add(TElement item)
		{
			_elements.Add(item);
		}

		public void AddRange(IEnumerable<TElement> items)
		{
			if (items.Any())
				_elements.AddRange(items);
		}



		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
