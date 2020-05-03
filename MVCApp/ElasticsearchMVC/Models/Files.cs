using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ElasticsearchMVC.Models
{
    public class Files
    {
        public string FilePath { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public int FileId { get; set; }
        public string ContentType { get; set; }
        public string Keywords { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        //public int UserId { get; set; }
        // public virtual User User { get; set; }


    }
}