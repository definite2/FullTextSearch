using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElasticsearchMVC.ViewModels;
using ElasticsearchMVC.Models;
using ElasticsearchMVC.Connectors;

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
            //List<SearchViewModel> viewModels = new List<SearchViewModel>();
            return View("Search"); //TODO
        }
        public JsonResult FileSearch(string search_string)
        {
            if (string.IsNullOrEmpty(search_string))
            {
                search_string = " ";
            }

            var responsedata = _connectionToEs.ESClient().Search<FilePath>
                (s => s.Source(sf => sf.Excludes(se => se.Fields(f=>f.Body))) //not return the body
                                 .Index("pdf_files")
                                 .Size(50)
                                 .Query(q => q
                                     .MatchPhrase(c => c
                        .Field(f => f.Body)
                        .Analyzer("standard")
                        .Query(search_string)
                        .Slop(2)
                        
                    )
                       ||
                    q.MatchPhrase(c => c
                       .Field(f => f.Author)
                       .Analyzer("standard")
                       .Query(search_string)
                       .Slop(2)

                    )
                       ||
                    q.MatchPhrase(c => c
                       .Field(f => f.Title)
                       .Analyzer("standard")
                       .Query(search_string)
                       .Slop(2)

                    )
                         ||
                    q.MatchPhrase(c => c
                       .Field(f => f.Keywords)
                       .Analyzer("standard")
                       .Query(search_string)
                       .Slop(2)

                    )
                     
                       ||
                    q.MatchPhrase(c => c
                       .Field(f => f.FileName)
                       .Analyzer("standard")
                       .Query(search_string)
                       .Slop(2)

                    )
                    )
                .Highlight(h => h.Fields(
                    x => x.Field(y => y.Keywords).HighlightQuery(q => q
                                                        .Match(m => m
                                                          .Field(p => p.Keywords)
                                                          .Query(search_string)
                                                        )),
                    x => x.Field(y => y.Author).HighlightQuery(q => q
                                                        .Match(m => m
                                                          .Field(p => p.Author)
                                                          .Query(search_string)
                                                        )),
                    x => x.Field(y => y.Body).HighlightQuery(q => q
                                                        .Match(m => m
                                                          .Field(p => p.Body)
                                                          .Query(search_string)
                                                        )),
                     x => x.Field(y => y.Title).HighlightQuery(q => q
                                                        .Match(m => m
                                                          .Field(p => p.Title)
                                                          .Query(search_string)
                                                        ))))
                                                    );

            var datasend = (from hits in responsedata.Hits
                            select hits.Source).ToList();

            return Json(new { datasend, responsedata.Took }, behavior: JsonRequestBehavior.AllowGet);

        }
    }
}