using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Departments
{
    public class ConsolidatedList
    {
        [Key]
        public int ConsolidatedListId { get; set; }
        public CollectionPoint CollectonPoint { get; set; }
        public Department Department { get; set; }
        public string ItemNumber { get; set; }
        public int ItemQty { get; set; }

        //  public DateTime RequestedDate { get; set; }
    }
}