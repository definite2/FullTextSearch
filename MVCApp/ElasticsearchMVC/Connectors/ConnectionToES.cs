using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nest;
using Elasticsearch.Net;

namespace ElasticsearchMVC.Connectors
{
    public class ConnectionToES
    {
        public ElasticClient ESClient()
        {
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200/"),
            };

            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
            var elasticClient = new ElasticClient(connectionSettings);

            return elasticClient;
        }

    }
}