using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.Interfaces
{
	public interface IMapReduceFileService
	{
		void CreateDirectory(String name);
		
		String CreateDocument(String directory, String name);
		String CreateDocument(String directory, String name, String content);
		String CreateDocument<T>(String directory, String name, T content);
		
		void WriteToDocument(String name, String content);

		List<String> ReadDocumentIds(String directory);

		String ReadDocument(String directory, String Id);
		T ReadDocument<T>(String directory, String Id) where T : new();
	}
}
