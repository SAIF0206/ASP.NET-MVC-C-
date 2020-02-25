using AD_Project.DbContext;
using AD_Project.Models.Store;
using AD_Project.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;


/*
Zhao Zheng Yun
 */

namespace AD_Project.Controllers
{
    [Authorize(Roles = "3")]
    public class StoreManagerController : Controller
    {
        private ApplicationDbContext _context;
        public StoreManagerController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Approve(int id)
        {

            var sts = _context.AdjustedItems.SingleOrDefault(m => m.AdjustedItemId == id);
            sts.status = "Approved";
            _context.SaveChanges();

            return RedirectToAction("adjVoucher", "StoreManager");
        }

        public ActionResult Reject(int id)
        {

            var sts = _context.AdjustedItems.SingleOrDefault(m => m.AdjustedItemId == id);
            sts.status = "Rejected";
            _context.SaveChanges();

            return RedirectToAction("adjVoucher", "StoreManager");
        }


        public ActionResult adjVoucher()
        {
            var adjitem = _context.AdjustedItems.Include(c => c.Item).Include(a => a.Suppliers_Prices).Where(b => b.status == "PendingManagerApproval").ToList();
            var Supp = _context.Suppliers_Prices.ToList();

            SampleViewModel sample = new SampleViewModel
            {
                Adjusteditem = adjitem,
                SuppliersPrice = Supp,
            };

            return View(sample);
        }
        public ActionResult SpecifySupplier()
        {
            List<Item> items = _context.Items.ToList();
            List<Supplier> suppliers = _context.Suppliers.ToList();
            List<Suppliers_Price> suppliers_Price = _context.Suppliers_Prices.ToList();

            var SupplierViewModel = new SupplierViewModel();
            SupplierViewModel.Items = items;
            SupplierViewModel.Suppliers = suppliers;
            SupplierViewModel.Suppliers_Price = suppliers_Price;


            return View(SupplierViewModel);
        }
        [HttpPost]

        public ActionResult TenderForm(SupplierViewModel supplierViewModel)
        {
            List<Item> items = _context.Items.ToList();
            List<Supplier> suppliers = _context.Suppliers.ToList();
            List<Suppliers_Price> suppliers_Prices = _context.Suppliers_Prices.ToList();

            var supplierList = _context.Suppliers_Prices.Include(m => m.Item).Include(a => a.Supplier)
                .Where(m => m.SupplierId == supplierViewModel.SelectedsupplierId).ToList();

            try
            {
                supplierViewModel.Suppliers_Price = supplierList;

                //  supplierViewModel.Suppliers_Price = suppliers_Prices;
                supplierViewModel.Suppliers = suppliers;
                supplierViewModel.Items = items;

                return View(supplierViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("DisburmentList");
            }

        }
        public JsonResult SendMailToUser()
        {

            bool result = false;

            result = SendEmail("storeclerkteam4@gmail.com", "Email From Store",
                "<p>Dear Store Clerk,<br/>Your Request for Adjustment voucher costs more than 250$ has been approved by the Store Manager." +
                "Best Wishes,<br/></p>");

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool SendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                //Update the key in webconfig <appsettings>
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

                //SSL Security
                client.EnableSsl = true;
                //Timeout
                client.Timeout = 100000;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.UseDefaultCredentials = false;

                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                //Mail Message

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);

                //If emailbody contains Html Tag
                mailMessage.IsBodyHtml = true;

                mailMessage.BodyEncoding = UTF8Encoding.UTF8;


                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public ActionResult TrendReport()
        {
            List<DataPoint> dataPoints2 = new List<DataPoint>();

            var dbPoints = _context.TrendAnalyses.ToList();
            foreach (var item in dbPoints)
            {
                dataPoints2.Add(new DataPoint(item.Date, item.Quantity));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints2);

            var viewModel = new ReportViewModel
            {

            };
            return View(viewModel);
        }

    }
}
