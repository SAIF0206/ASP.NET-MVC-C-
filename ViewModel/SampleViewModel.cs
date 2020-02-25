using AD_Project.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Project.ViewModel
{
    public class SampleViewModel
    {
        public List<AdjustedItem> Adjusteditem { get; set; }
        public List<Suppliers_Price> SuppliersPrice { get; set; }
    }
}