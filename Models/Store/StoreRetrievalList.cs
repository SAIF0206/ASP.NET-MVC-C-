using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class StoreRetrievalList
    {
        [Key]
        public int Id { get; set; }
        public Item item { get; set; }
        public int ItemId { get; set; }
        public int TotalNeededQty { get; set; }

        public int StockQty { get; set; }
        

        public ICollection<DeptRequestedItems> DeptRequestedItems { get; set; }
    }
}