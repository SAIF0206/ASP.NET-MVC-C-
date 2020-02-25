using AD_Project.DbContext;
using AD_Project.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

/*
Ruan Xinping
 */

namespace AD_Project.Controllers
{
    [Authorize(Roles = "2")]
    public class StoreSupervisorController : Controller
    {
        private ApplicationDbContext _context;
        public StoreSupervisorController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult AppRejVoucher()
        {
            var adjitem = _context.AdjustedItems.Include(c => c.Item).Include(a => a.Suppliers_Prices).Where(b => b.status == "Pending").ToList();
            var Supp = _context.Suppliers_Prices.ToList();

            SampleViewModel sample = new SampleViewModel
            {
                Adjusteditem = adjitem,
                SuppliersPrice = Supp,
            };

            return View(sample);
        }
        public ActionResult Approve(int id)
        {

            var sts = _context.AdjustedItems.SingleOrDefault(m => m.AdjustedItemId == id);
            sts.status = "Approved";
            _context.SaveChanges();
            return RedirectToAction("AppRejVoucher", "StoreSupervisor");
        }
        public ActionResult Reject(int id)
        {

            var sts = _context.AdjustedItems.SingleOrDefault(m => m.AdjustedItemId == id);
            sts.status = "Rejected";
            _context.SaveChanges();
            return RedirectToAction("AppRejVoucher", "StoreSupervisor");
        }
        public ActionResult PendingManagerApproval(int id)
        {

            var sts = _context.AdjustedItems.SingleOrDefault(m => m.AdjustedItemId == id);
            sts.status = "PendingManagerApproval";
            _context.SaveChanges();
            return RedirectToAction("AppRejVoucher", "StoreSupervisor");
        }
        public JsonResult SendMailToUser()
        {

            bool result = false;

            result = SendEmail("storeclerkteam4@gmail.com", "Email From Store",
                "<p>Dear Store Clerk,<br/>Your Request for Adjustment voucher costs less than 250$ has been approved by the Store Supervisor." +
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

    }
}
