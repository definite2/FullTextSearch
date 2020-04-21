using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;



namespace ElasticsearchTika
{
    public class ESClient
    {
        public string indexName { get; set; }
     
        public ElasticClient elastic()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex(indexName);
            
            var client = new ElasticClient(settings);
          
            return client;

        }
     
     


    
    }
}