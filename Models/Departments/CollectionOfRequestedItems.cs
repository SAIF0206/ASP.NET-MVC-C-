using AD_Project.Models.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Departments
{
    public class CollectionOfRequestedItems
    {
        [Key]
        public int CollectionOfRequestedItemsId { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int TotalRequestedQty { get; set; }

        [ForeignKey("ConsolidatedReq")]
        public int ConsolidatedRequisitionId { get; set; }
        public ConsolidatedRequisition ConsolidatedReq { get; set; }
    }
}