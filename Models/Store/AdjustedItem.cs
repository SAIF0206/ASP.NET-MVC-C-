using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using AD_Project.Models.Departments;

namespace AD_Project.Models.Store
{
    public class AdjustedItem
    {
        [Key]
        public int AdjustedItemId { get; set; }
        public Item Item { get; set; }
        [Display(Name ="Reason")]
        public String AdjustmentReason { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }      
        public int AdjustedQty { get; set; }
        public String status { get; set; }
        public Suppliers_Price Suppliers_Prices { get; set; }
       

    }
}