using AD_Project.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Project.ViewModel
{
    public class SupplierViewModel
    {
        public int SelectedsupplierId { get; set; }
        public List<Item> Items { get; set; }
    //    public Item iitems { get; set; }

        public List<Supplier> Suppliers { get; set; }
        public List<Suppliers_Price> Suppliers_Price { get; set; }

        public Suppliers_Price ssuppliers_price { get; set; }
    }
}