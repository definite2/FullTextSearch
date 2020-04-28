using System;

namespace ElasticsearchTika
{
    public class Files
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedDate { get; set; }
             public DateTime ModifiedDate { get; set; }


    }


}
