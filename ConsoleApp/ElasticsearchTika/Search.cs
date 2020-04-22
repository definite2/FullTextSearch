using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;


namespace ElasticsearchTika
{
    public class Search
    {
        public ElasticClient es { get; set; }
        public SearchResult Search_get(string query)
        {
            var searchResult = new SearchResult()
            {
                Query = query
            };
            var items = new List<SearchResultItem>();

            if (query.Contains(" "))
            {
                SearchPhrase(query, items);
            }

            SearchFuzzy(query, items);

            searchResult.Items = items.ToArray();
            return searchResult;
        }
        //source filtering exclude kullanıldı, search response'undan body çıkarıldı. hızlı return olması için
        void SearchFuzzy(string query, IList<SearchResultItem> items)
        {
            var response = es.Search<Files>(s => s.Source(sf=>sf.Excludes(e=>e.Fields(f=>f.Body)))
                .From(0)
                .Size(10)
                .Query(q => q
                    .Fuzzy(m => m
                        .Field(f => f.Body)
                        .Value(query)))
                .Highlight(h => h.Fields(x => x.Field(y => y.Body))));

            if (response.Documents.Count > 0)
            {
                
                foreach (var hit in response.Hits)
                {
                    //Console.WriteLine("FILE: " + hit.Source.Title);

                    foreach (var highlight in hit.Highlight)
                    {
                        foreach (var highlightItem in highlight.Value)
                        {
                            items.Add(new SearchResultItem()
                            {
                                FilePath = hit.Source.FilePath,
                                Highlight = highlightItem
                            });
                        }
                    }
                }
            }
        }
      void SearchPhrase(string phrase, IList<SearchResultItem> items)
        {
            var response = es.Search<Files>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .MatchPhrase(c => c
                        .Analyzer("standard")
                        .Boost(1.1)
                        .Query(phrase)
                        .Slop(2)
                        .Field(f => f.Body)
                    ))
                .Highlight(h => h.Fields(x => x.Field(y => y.Body))));

            if (response.Documents.Count > 0)
            {
                
                foreach (var hit in response.Hits)
                {
                    //Console.WriteLine("FILE: " + hit.Source.Title);

                    foreach (var highlight in hit.Highlight)
                    {
                        foreach (var highlightItem in highlight.Value)
                        {
                            if (items.Count > 50)
                                return;

                            items.Add(new SearchResultItem()
                            {
                                FilePath = hit.Source.FilePath,
                                Highlight = highlightItem
                            });
                        }
                    }
                }
            }

        }
    }
}
