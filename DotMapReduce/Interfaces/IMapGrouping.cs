using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapGrouping<TKey, TElement> : IGrouping<TKey, TElement>
	{
		void Add(TElement item);
		void AddRange(IEnumerable<TElement> items);
	}

}
