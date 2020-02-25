using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class Suppliers_Price
    {
        [Key]
        public int SuppliersPriceId { get; set; }
      //  [ForeignKey("Item")]
        public int Itemid { get; set; }
        public Item Item { get; set; }
        
        public int ItemPrice { get; set; }
        public Supplier Supplier { get; set; }
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }


    }
}