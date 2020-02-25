using AD_Project.Models.Departments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class Store_Staff : User
    {
        public Store store { get; set; }

        [ForeignKey("store")]
        public int storeId { get; set; }
    }
}