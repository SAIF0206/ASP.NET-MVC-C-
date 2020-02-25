using AD_Project.DbContext;
using AD_Project.Models.Store;

using AD_Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

/*
Zhao Min
 */

namespace AD_Project.Controllers
{
    [Authorize(Roles = "1")]
    public class SupplierController : Controller
    {
        private ApplicationDbContext _context;

        public SupplierController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult PurchaseOrder()
        {
            List<Item> items = _context.Items.ToList();
            List<Supplier> suppliers = _context.Suppliers.ToList();
            List<Suppliers_Price> suppliers_Price = _context.Suppliers_Prices.ToList();

            var PurchaseOrderViewModel = new PurchaseOrderViewModel();
            PurchaseOrderViewModel.Items = items;
            PurchaseOrderViewModel.Suppliers = suppliers;
            PurchaseOrderViewModel.SuppliersPrice = suppliers_Price;


            return View(PurchaseOrderViewModel);
        }

        public ActionResult SpecifySupplier(PurchaseOrderViewModel purchaseOrderViewModel)
        {
            List<Item> items = _context.Items.ToList();
            List<Supplier> suppliers = _context.Suppliers.ToList();
            List<Suppliers_Price> suppliers_Prices = _context.Suppliers_Prices.ToList();
            var itemsOfSupplier = _context.Suppliers_Prices.Include(d => d.Item).Include(g => g.Supplier)
                .Where(f => f.SupplierId == purchaseOrderViewModel.SelectedsupplierId);

            try
            {
                purchaseOrderViewModel.SuppliersPrice = itemsOfSupplier;
                purchaseOrderViewModel.Suppliers = suppliers;
                purchaseOrderViewModel.Items = items;

                return View("PurchaseOrder", purchaseOrderViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("DisburmentList");
            }

        }

        public ViewResult Low_Stock()
        {
            List<Item> item = new List<Item>();
            var low = _context.Items.Include(c => c.Stockdetails).ToList();

            foreach (var i in low)
            {
                foreach (var j in i.Stockdetails)
                {
                    if (j.InventoryStockQty < i.ReorderLevel)
                    {
                        item.Add(i);
                    }
                }
            }
            return View(item);
        }
        public ActionResult Generate()
        {
            var supplierName = _context.Suppliers.ToList();
            var viewModel = new PurchaseOrderViewModel
            {
                Suppliers = supplierName
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            return View();
        }
    }
}
