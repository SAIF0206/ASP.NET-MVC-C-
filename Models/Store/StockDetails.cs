using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class StockDetails
    {
        [Key]
        public int StockDetailsId { get; set; }
        public Item item { get; set; }
        [ForeignKey("item")]
        public int ItemId { get; set; }
        public int InventoryStockQty { get; set; }
        public int ItemQtyChanged { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}