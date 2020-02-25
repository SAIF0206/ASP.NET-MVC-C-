using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class Store
    {
        [Key]
        public int storeId { get; set; }
       public string StoreName { get; set; }
    }
}