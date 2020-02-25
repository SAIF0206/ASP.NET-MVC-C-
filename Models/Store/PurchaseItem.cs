using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class PurchaseItem
    {
        [Key]
        public int PurchaseItemId { get; set; }
        public Item Item { get; set; }
        public int PurchaseQty { get; set; }
    }
}