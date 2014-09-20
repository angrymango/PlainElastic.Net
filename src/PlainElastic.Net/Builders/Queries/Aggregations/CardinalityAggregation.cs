using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PlainElastic.Net.Utils;

namespace PlainElastic.Net.Queries
{
    /// <summary>
	/// A single-value metrics aggregation that calculates an approximate count of distinct values. Values can be extracted either from specific fields in the document or generated by a script.
	/// see http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-aggregations-metrics-cardinality-aggregation.html
	/// </summary>
	public class CardinalityAggregation<T> : AggregationBase<CardinalityAggregation<T>, T>
    {
        private readonly List<string> aggregationFields = new List<string>();

        /// <summary>
		/// The field to execute cardinality aggregation against.
        /// </summary>
		public CardinalityAggregation<T> Field(string fieldName)
        {
            RegisterJsonPart("'field': {0}", fieldName.Quotate());            
            return this;
        }

        /// <summary>
		/// The field to execute cardinality aggregation against.
        /// </summary>
		public CardinalityAggregation<T> Field(Expression<Func<T, object>> field)
        {
            return Field(field.GetPropertyPath());
        }

        /// <summary>
		/// The field to execute cardinality aggregation against.
        /// </summary>
		public CardinalityAggregation<T> FieldOfCollection<TProp>(Expression<Func<T, IEnumerable<TProp>>> collectionField, Expression<Func<TProp, object>> field)
        {
            var collectionProperty = collectionField.GetPropertyPath();
            var fieldName = collectionProperty + "." + field.GetPropertyPath();

            return Field(fieldName);
        }

		/// <summary>
		/// The precision_threshold options allows to trade memory for accuracy, and defines a unique count below which counts are expected to be close to accurate. Above this value, counts might become a bit more fuzzy. The maximum supported value is 40000, thresholds above this number will have the same effect as a threshold of 40000. Default value depends on the number of parent aggregations that multiple create buckets (such as terms or histograms).
		/// </summary>
		public CardinalityAggregation<T> PrecisionThreshold(int precision_threshold)
		{
			RegisterJsonPart("'precision_threshold': {0}", precision_threshold.AsString());
			return this;
		}

		/// <summary>
		/// If you computed a hash on client-side, stored it into your documents and want Elasticsearch to use them to compute counts using this hash function without rehashing values, it is possible to specify rehash: false. Default value is true. Please note that the hash must be indexed as a long when rehash is false.
		/// </summary>
		public CardinalityAggregation<T> Rehash(bool rehash)
		{
			RegisterJsonPart("'rehash': {0}", rehash.AsString());
			return this;
		}

        /// <summary>
		/// Allow to define a script to evaluate, with its value used to compute the cardinality information.
        /// </summary>
		public CardinalityAggregation<T> Script(string script)
        {
            RegisterJsonPart("'script': {0}", script.Quotate());
            return this;
        }


        /// <summary>
        /// Sets a scripting language used for scripts.
        /// By default used mvel language.
        /// see: http://www.elasticsearch.org/guide/reference/modules/scripting.html
        /// </summary>
		public CardinalityAggregation<T> Lang(string lang)
        {
            RegisterJsonPart("'lang': {0}", lang.Quotate());
            return this;
        }

        /// <summary>
        /// Sets a scripting language used for scripts.
        /// By default used mvel language.
        /// see: http://www.elasticsearch.org/guide/reference/modules/scripting.html
        /// </summary>
		public CardinalityAggregation<T> Lang(ScriptLangs lang)
        {
            return Lang(lang.AsString());
        }

        /// <summary>
        /// Sets parameters used for scripts.
        /// </summary>
		public CardinalityAggregation<T> Params(string paramsBody)
        {
            RegisterJsonPart("'params': {0}", paramsBody);
            return this;
        }

        protected override string ApplyAggregationBodyJsonTemplate(string body)
        {
            return "'cardinality': {{ {0} }}".AltQuoteF(body);
        }
        
    }
}