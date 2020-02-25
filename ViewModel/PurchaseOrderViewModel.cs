using AD_Project.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Project.ViewModel
{
    public class PurchaseOrderViewModel
    {
        public int SelectedsupplierId { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
        public IEnumerable<PurchaseOrder> PurchaseOrders { get; set; }
        public StockDetails StockDetails { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public Item iitems { get; set; }
        public IEnumerable<Suppliers_Price> SuppliersPrice { get; set; }
    }
}