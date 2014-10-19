using DotMapReduce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce
{
	public class MemoryFileSerivce : IMapReduceFileService
	{
		public MemoryFileSerivce()
		{

		}

		public MemoryFileSerivce(IEnumerable<String> values)
		{

		}

		public void CreateDirectory(string name)
		{
			throw new NotImplementedException();
		}

		public string CreateDocument(string directory, string name)
		{
			throw new NotImplementedException();
		}

		public string CreateDocument(string directory, string name, string content)
		{
			throw new NotImplementedException();
		}

		public string CreateDocument<T>(string directory, string name, T content)
		{
			throw new NotImplementedException();
		}

		public void WriteToDocument(string name, string content)
		{
			throw new NotImplementedException();
		}

		public List<string> ReadDocumentIds(string directory)
		{
			throw new NotImplementedException();
		}

		public string ReadDocument(string directory, string Id)
		{
			throw new NotImplementedException();
		}

		public T ReadDocument<T>(string directory, string Id) where T : new()
		{
			throw new NotImplementedException();
		}
	}
}
