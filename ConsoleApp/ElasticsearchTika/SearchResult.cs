using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticsearchTika
{
    public class SearchResult
    {
        public string Query { get; set; }
        public SearchResultItem[] Items { get; set; }
    }
}
