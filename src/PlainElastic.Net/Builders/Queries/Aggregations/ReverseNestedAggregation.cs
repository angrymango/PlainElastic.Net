using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PlainElastic.Net.Utils;

namespace PlainElastic.Net.Queries
{
	/// <summary>
	/// A special single bucket aggregation that enables aggregating on parent docs from nested documents. Effectively this aggregation can break out of the nested block structure and link to other nested structures or the root document, which allows nesting other aggregations that aren’t part of the nested object in a nested aggregation.
	/// The reverse_nested aggregation must be defined inside a nested aggregation.
	/// see http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-aggregations-bucket-reverse-nested-aggregation.html
	/// </summary>
	public class ReverseNestedAggregation<T> : AggregationBase<ReverseNestedAggregation<T>, T>
	{
		/// <summary>
		/// The path to reverse nest.
		/// </summary>
		public ReverseNestedAggregation<T> Path(string path)
		{
			RegisterJsonPart("'path': {0}", path.Quotate());            
			return this;
		}

		protected override string ApplyAggregationBodyJsonTemplate(string body)
		{
			if (string.IsNullOrEmpty(body))
			{
				return "'reverse_nested': {{ {0} }}".AltQuoteF(body);
			}

			return "'reverse_nested': {}".AltQuote();
		}

	}
}
