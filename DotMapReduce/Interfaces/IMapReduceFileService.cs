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
		void CreateDocument(String name);
		void CreateDocument(String name, String content);
		
		void WriteToDocument(String name, String content);

		List<String> ReadDocumentIds(String directory);
		String ReadDocument(String Id);
	}
}
