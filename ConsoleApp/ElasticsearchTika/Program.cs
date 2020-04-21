using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TikaOnDotNet.TextExtraction;
using Nest;

namespace ElasticsearchTika
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = @"C:\Users\defne.eroglu\Documents\ElasticsearchPoC";
            string[] dirs = Directory.GetDirectories(rootPath);

            string[] files = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories);
            Tika tika = new Tika();
            tika._cut = new TextExtractor();
            ESClient es = new ESClient();
            string _IndexName = "comp_assignments";
            es.indexName = _IndexName;

            var client = es.elastic();

            var only_these_extensions = new[] { ".pdf", ".rtf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".csv" };

            client.Indices.Create(_IndexName, c => c.Index(_IndexName).Map<Files>(t => t.AutoMap()));
            //client.Indices.Create(_IndexName, c => c.Map<File>(t => t.AutoMap()));



            foreach (string file in files)
            {
                if (only_these_extensions.Any(f => f == Path.GetExtension(file)))
                {
                    try
                    {
                        tika.file = file;
                        var test = tika.tika_metadata();
                        var indexresult = client.IndexDocument<Files>(test);
                        Console.WriteLine(indexresult.Id);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                        continue;
                    }
                }
            }
            Console.ReadLine();
        }
    }

}
