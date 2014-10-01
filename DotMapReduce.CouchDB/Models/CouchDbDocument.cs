using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.CouchDB
{
	public interface ICouchDbDocument
	{
		String Id { get; set; }
		String Rev { get; set; }
	}

	public class CouchDbDocument : ICouchDbDocument
	{
		public String Id { get; set; }
		public String Rev { get; set; }
	}

	public class DbDocs
	{
		public Int32 TotalRows { get; set; }
		public List<CouchDbDocument> Rows { get; set; }
	}
}
