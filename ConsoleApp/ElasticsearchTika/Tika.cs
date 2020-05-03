using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Elasticsearch.Net;

using TikaOnDotNet.TextExtraction;
using System.IO;

using org.apache.tika.io;
using org.apache.commons.io.output;

namespace ElasticsearchTika
{
    public class Tika
    {
        public TextExtractor _cut;
        public  string file { get; set; }
        public   Files tika_metadata()
        {

            var result = _cut.Extract(file);
            var files = new Files()
            {
                Id = Path.GetFileName(file),
                ContentType = result.ContentType,
                Body = result.Text,
                FilePath = Path.GetDirectoryName(file),
                CreatedDate = File.GetCreationTime(file),
                ModifiedDate = File.GetLastWriteTime(file)  
                
               
            };
            if (result.Metadata.ContainsKey("title"))
                files.Title = result.Metadata["title"];
            else
                files.Title = file;
            if (result.Metadata.ContainsKey("Content-Length"))
            {
                long longtest;
                if (long.TryParse(result.Metadata["Content-Length"], out longtest))
                {
                    files.ContentLength = longtest;
                }
            }
            if (result.Metadata.ContainsKey("Author"))
            {
                files.Author = result.Metadata["Author"];
            }
          
            return files;
        }

    }
}

