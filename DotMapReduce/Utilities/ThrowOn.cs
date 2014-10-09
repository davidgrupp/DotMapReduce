using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Utilities
{
	public static class ThrowOn
	{
		public static void IsNullOrEmpty(String obj, String errorMessage, Boolean allowWhiteSpace = false)
		{
			if ((!allowWhiteSpace && String.IsNullOrWhiteSpace(obj)) || String.IsNullOrEmpty(obj))
			{
				throw new ArgumentException(errorMessage);
			}
		}

		public static void IsEmpty<T>(IEnumerable<T> obj, String errorMessage)
		{
			IsNull(obj, errorMessage);
			if (0 >= obj.Count())
			{
				throw new ArgumentException(errorMessage);
			}
		}

		public static void IsEmpty(Guid value, String errorMessage)
		{
			if (value == Guid.Empty)
			{
				throw new ArgumentNullException(errorMessage);
			}
		}

		public static void IsNull(Object obj, String errorMessage)
		{
			if (null == obj)
			{
				throw new ArgumentNullException(errorMessage);
			}
		}
		public static void IsNotType<T>(Object obj, String errorMessage)
		{
			if (!(obj is T))
			{
				throw new ArgumentException(errorMessage);
			}
		}

		public static void IsNotType<T>(Type type, String errorMessage)
		{
			if (typeof(T) != type)
			{
				throw new ArgumentException(errorMessage);
			}
		}
	}
}
