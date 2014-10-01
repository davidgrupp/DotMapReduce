using DotMapReduce.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.CouchDB
{
	public class CouchMapReduceFileService : IMapReduceFileService
	{

		public CouchMapReduceFileService()
		{
			_client = new RestClient("http://localhost:5984");
		}

		private IRestClient _client;

		public void CreateDirectory(String name)
		{
			var request = new RestRequest(name);
			var response = _client.Put(request);
		}
		public String CreateDocument(String directory, String name)
		{
			var request = new RestRequest(String.Format("/{0}/", directory));
			request.RequestFormat = DataFormat.Json;
			request.AddBody(new { });
			var response = _client.Post<CouchDbDocument>(request);
			return response.Data.Id;
		}

		public String CreateDocument(String directory, String name, String content)
		{
			var request = new RestRequest(String.Format("/{0}/", directory));
			request.RequestFormat = DataFormat.Json;
			request.AddBody(new { Content = content });
			var response = _client.Post<CouchDbDocument>(request);
			return response.Data.Id;
		}

		public String CreateDocument<T>(String directory, String name, T content)
		{
			var request = new RestRequest(String.Format("/{0}/", directory));
			request.RequestFormat = DataFormat.Json;
			request.AddBody(content);
			var response = _client.Post<CouchDbDocument>(request);
			return response.Data.Id;
		}

		public void WriteToDocument(String name, String content)
		{

		}

		public List<String> ReadDocumentIds(String directory)
		{
			var request = new RestRequest(String.Format("/{0}/_all_docs", directory));
			var response = _client.Get<DbDocs>(request);
			return response.Data.Rows.Select(r => r.Id).ToList();
		}
		public String ReadDocument(String directory, String Id)
		{
			var request = new RestRequest(String.Format("/{0}/{1}", directory, Id));
			var response = _client.Get<CouchDbDocument>(request);
			return response.Content;
		}

		public T ReadDocument<T>(String directory, String Id) where T : new()
		{
			var request = new RestRequest(String.Format("/{0}/{1}", directory, Id));
			var response = _client.Get<T>(request);
			return response.Data;
		}

		
	}

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

	public class BaseballDoc : CouchDbDocument
	{
		public String prices { get; set; }
		public List<Int32> p { get; set; }
	}

	public class DbDocs
	{
		public Int32 TotalRows { get; set; }
		public List<DbDocId> Rows { get; set; }
	}

	public class DbDocId
	{
		public String Id { get; set; }
		public String Key { get; set; }
	}
}
