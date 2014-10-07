using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Utilities
{
	public static class PartitionUtilities
	{
		public static Int32 GetPartition(String key, Int32 max)
		{
			var keyHash = DistributedHash(key) % max;
			return keyHash;
		}

		/// <summary>
		/// Results in better distribution of keys by scrambling the result hash value of the input string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static Int32 DistributedHash(String key)
		{
			var rand = new Random(key.GetHashCode());
			return rand.Next();
		}
	}
}
