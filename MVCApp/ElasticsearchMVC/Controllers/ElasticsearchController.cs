using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElasticsearchMVC.Models;
using ElasticsearchMVC.Connectors;
using Elasticsearch.Net;
using Nest;
using System.IO;
using System.Dynamic;
namespace ElasticsearchMVC.Controllers
{
    public class ElasticsearchController : Controller
    {
        // GET: Elasticsearch
        private readonly ConnectionToES _connectionToEs;
        public ElasticsearchController()
        {
            _connectionToEs = new ConnectionToES();
        }

        [HttpGet]
        public ActionResult Search()
        {
        
            return View("Search");
        }
        public JsonResult FileSearch(string search)
        {
         //   if (!string.IsNullOrEmpty(search_string))
            //{          
                var responsedata = _connectionToEs.ESClient().Search<Files>
                    (s => s.Source(sf => sf.Excludes(se => se.Fields(f => f.Body))) 
                                     .Index("may_kivrik")                           
                                     .Size(50)
                                     .Query(q => q
                                         .MatchPhrase(m => m
                                                      .Field(f => f.FilePath)
                                                      .Query(search)
                                                      .Analyzer("standard")
                                                      .Slop(2)
                                                 )
                                                 ||
                                                 q.MatchPhrase(m => m
                                                      .Field(f => f.Author)
                                                      .Query(search)
                                                      .Analyzer("standard")
                                                      .Slop(2)
                                                 )
                                                 ||
                                                 q.MatchPhrase(m => m
                                                      .Field(f => f.Keywords)
                                                      .Query(search)
                                                      .Analyzer("standard")
                                                      .Slop(2)
                                                 )
                                                 ||
                                                 q.MatchPhrase(m => m
                                                      .Field(f => f.Body)
                                                      .Query(search)
                                                      .Analyzer("standard")
                                                      .Slop(2)
                                                 )
                                                 )
                                      );
       
            var datasend = (from hits in responsedata.Hits
                                 select hits.Source).ToList();

           
            return Json(new { datasend, responsedata.Took }, behavior: JsonRequestBehavior.AllowGet);


        }
    }
}
