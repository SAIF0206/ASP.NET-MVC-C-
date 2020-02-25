using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class DisburseItem
    {
        [Key]
        public int DisburseItemId { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int DisburseQty { get; set; }


    }
}