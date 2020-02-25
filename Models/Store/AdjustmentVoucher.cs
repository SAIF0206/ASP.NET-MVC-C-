using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class AdjustmentVoucher
    {
        [Key]
        public int AdjustmentVoucherId { get; set; }
        public DateTime IssuedDate { get; set; }
       public ICollection<AdjustedItem> AdjustedItems { get; set; }


    }
}