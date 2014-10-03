using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotMapReduce.CouchDB.Utilities
{
	internal class ObjectExpressionVisitor : ExpressionVisitor
	{
		public static List<String> GetPath<TSource>(Expression<Func<TSource, Object>> expression)
		{
			var visitor = new ObjectExpressionVisitor();
			visitor.Visit(expression.Body);
			return Enumerable.Reverse(visitor._path).ToList();
		}
		public static List<String> GetPath<TSource>(Expression<Func<TSource, IEnumerable>> expression)
		{
			var visitor = new ObjectExpressionVisitor();
			visitor.Visit(expression.Body);
			return Enumerable.Reverse(visitor._path).ToList();
		}
		private readonly List<String> _path = new List<String>();
		protected override Expression VisitMember(MemberExpression node)
		{
			_path.Add(node.Member.Name);
			return base.VisitMember(node);
		}
	}
}
