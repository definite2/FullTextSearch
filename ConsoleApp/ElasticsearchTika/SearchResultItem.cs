using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticsearchTika
{
    public class SearchResultItem
    {
        public string FilePath { get; set; }
        public string Highlight { get; set; }
    }
}
