using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ElasticsearchMVC.Models
{
    public class FilePath:Model
    {

        public int FilePathId { get; set; }
        
        public string FileName { get; set; }
       
        public string Author { get; set; }
    
        public string Keywords { get; set; } //Açıklama
       
        public DateTime CreatedDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
    }
}