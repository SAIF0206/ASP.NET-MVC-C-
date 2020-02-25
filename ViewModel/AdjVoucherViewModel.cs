using AD_Project.Models.Store;
using System.Collections.Generic;

namespace AD_Project.ViewModel
{
    public class AdjVoucherViewModel
    {
        public List<AdjustedItem> AdjustedItems { get; set; }
        public List<Item> Item { get; set; }
        public List<Suppliers_Price> SuppliersPrice { get; set; }

    }
}
