using AD_Project.DbContext;
using AD_Project.Models.Departments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

/*
Zhao Min
 */

namespace AD_Project.Controllers
{
    [Authorize(Roles = "6")]
    public class HeadController : Controller
    {
        private ApplicationDbContext _context;
        public HeadController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ViewResult PendingForApproval()
        {
            var LoggedInUserhead = (Dept_Staff)Session["loginUser"];
            var pendingForApproval = _context.EmployeeRequestForms.Include(c => c.DeptStaff).Where(m => m.EmployeeRequestFormstatus == "Pending" && m.DeptId == LoggedInUserhead.DeptId).ToList();
            return View(pendingForApproval);
        }

        public ActionResult RequestDetails(int id)
        {
            var details = _context.EmployeeRequestForms.Include(i => i.RequestItems.Select(it => it.Item))
                .SingleOrDefault(m => m.EmployeeRequestFormId == id);
            return View(details);
        }
        public ActionResult Approve(int id)
        {

            var sts = _context.EmployeeRequestForms.SingleOrDefault(m => m.EmployeeRequestFormId == id);
            sts.EmployeeRequestFormstatus = "Approved";
            _context.SaveChanges();

            return RedirectToAction("PendingForApproval", "Head");
        }

        public ActionResult Reject(int id)
        {

            var sts = _context.EmployeeRequestForms.SingleOrDefault(m => m.EmployeeRequestFormId == id);
            sts.EmployeeRequestFormstatus = "Rejected";
            _context.SaveChanges();

            return RedirectToAction("PendingForApproval", "Head");
        }
        public ActionResult AssignDelegate()
        {
            var user = (Dept_Staff)Session["loginUser"];
            var assign = _context.Dept_Staffs.ToList();
            List<Dept_Staff> depts = new List<Dept_Staff>();
            foreach (var i in assign)
            {
                if (i.Staff_Status.Equals("Available") && i.DeptId == user.DeptId)
                {
                    if (i.Username.StartsWith("head") != true)
                    {
                        depts.Add(i);
                    }

                }
            }

            return View(depts);
        }

        [HttpPost]
        public ActionResult Assign(HeadDelegate f)
        {
            var sts = _context.Dept_Staffs.SingleOrDefault(m => m.UserId == f.UserId);
            sts.Staff_Status = "Assigned";
            if (sts.Role == "4")
            {
                sts.Role = "7";
            }
            else if (sts.Role == "5")
            {
                sts.Role = "8";
            }

            HeadDelegate del = new HeadDelegate
            {
                UserId = f.UserId,
                StartDate = f.StartDate,
                EndDate = f.EndDate
            };
            _context.HeadDelegates.Add(del);
            _context.SaveChanges();

            return Json(0);
        }

        public ActionResult EditDelegate()
        {
            var a = _context.Dept_Staffs.ToList();
            List<Dept_Staff> dept = new List<Dept_Staff>();
            foreach (var i in a)
            {
                if (i.Staff_Status.Equals("Assigned"))
                {
                    dept.Add(i);
                }
            }
            return View(dept);
        }

        [HttpGet]
        public ActionResult DelegateDetail(int id)
        {
            var detail = _context.Dept_Staffs.SingleOrDefault(m => m.UserId == id);
            detail.Staff_Status = "Available";
            _context.SaveChanges();
            return RedirectToAction("EditDelegate", "Head");
        }

        [HttpPost]
        public ActionResult DelegateDetail()
        {
            return RedirectToAction("EditDelegate", "Head");
        }

        public ActionResult Send()
        {
            return RedirectToAction("PendingForApproval", "Head");
        }
        //Email
        public JsonResult SendMailToUser()
        {

            bool result = false;

            result = SendEmail("deptemployeeteam4@gmail.com", "Email From Department",
                "<p>Dear Employee,<br/>Your Request for items has been approved by the department Head." +
                "Best Wishes,<br/></p>");

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendRejectionMailToUser()
        {

            bool result = false;

            result = SendEmail("deptemployeeteam4@gmail.com", "Email From Department",
                "<p>Dear Employee,<br/>Your Request for items has not been approved by the department Head." +
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
        public ActionResult AssignRep()
        {
            var user = (Dept_Staff)(Session["loginUser"]);
            if (Session["loginUser"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var assign = _context.Dept_Staffs.ToList();
                List<Dept_Staff> depts = new List<Dept_Staff>();
                foreach (var i in assign)
                {
                    if ((i.Role.Equals("5") || i.Role.Equals("8")) && i.DeptId == user.DeptId)
                    {
                        depts.Add(i);
                    }
                }

                return View(depts);
            }

        }

        public ActionResult ChangeRepList()
        {
            var user = (Dept_Staff)(Session["loginUser"]);
            var a = _context.Dept_Staffs.ToList();
            List<Dept_Staff> dept = new List<Dept_Staff>();
            if (Session["loginUser"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var i in a)
                {
                    if ((i.Role.Equals("4") || i.Role.Equals("7")) && i.DeptId == user.DeptId)
                    {
                        dept.Add(i);
                    }
                }

            }

            return View(dept);
        }

        public ActionResult AssignDetail(int id)
        {
            var detail = _context.Dept_Staffs.SingleOrDefault(m => m.UserId == id);
            if (detail.Role == "4")
            {
                detail.Role = "5";
            }
            else if (detail.Role == "7")
            {
                detail.Role = "8";
            }

            _context.SaveChanges();
            return RedirectToAction("AssignRep", "Head");
        }

        public ActionResult CancelRep(int id)
        {
            var detail = _context.Dept_Staffs.SingleOrDefault(m => m.UserId == id);
            if (detail.Role == "5")
            {
                detail.Role = "4";
            }
            else if (detail.Role == "8")
            {
                detail.Role = "7";
            }

            _context.SaveChanges();
            return RedirectToAction("AssignRep", "Head");
        }
    }
}
