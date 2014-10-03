using DotMapReduce.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

		public void WriteToDocument<T>(String directory, String Id, T content, Expression<Func<T, Object>> expression)
		{
			var expressionParts = Utilities.ObjectExpressionVisitor.GetPath<T>(expression);
			var jsExpression = expressionParts.Aggregate((s1, s2) => String.Format("{0}.{1}", s1, s2));

			//var request = new RestRequest(String.Format("/{0}/_design/app/_update/setfield/{1}?field=name&value=JP", directory, Id));
			var request = new RestRequest(String.Format("/{0}/_design/app/_update/setfield/{1}", directory, Id));
			request.AddParameter("expression", jsExpression);
			request.AddParameter("value", request.JsonSerializer.Serialize(content));
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


}
