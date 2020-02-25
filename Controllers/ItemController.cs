using AD_Project.DAO;
using AD_Project.DbContext;
using AD_Project.Models.Departments;
using AD_Project.Models.Store;
using AD_Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

/*
    MOHD SAIF ANSARI
*/

namespace AD_Project.Controllers
{
    [Authorize(Roles = "5,8,10")]
    public class ItemController : Controller

    {
        private ApplicationDbContext db;
        private ConsolidatedRequestsService daoObj;
        private DepartmentService deptService;
        private AcknowledgeService ackService;
        public ItemController()
        {
            db = new ApplicationDbContext();
        }

        //Add Items to Catalogue
        public ActionResult Catalogue(string query)
        {
            List<Item> items = db.Items.ToList();
            List<Item> newItems = new List<Item>();
            if (query != null)
            {
                var stationery = items.Where(x => x.ItemDesc.ToLower().Contains(query.ToLower())).ToList();
                return View(stationery);
            }
            else
            {
                //Session to store information and the work done by particular user
                if (Session["rForm"] != null)
                {
                    List<RequestItem> itemSession = (List<RequestItem>)Session["rForm"];

                    foreach (var j in itemSession)
                    {
                        var item = items.Find(m => m.Id == j.Item.Id);
                        items.Remove(item);
                    }
                }

                return View(items);
            }
        }



        //Add to Requisition Form using Catalogue
        public ActionResult AddToRequisitionForm(int itemId, string qty)
        {
            var item = db.Items.Find(itemId);
            RequestItem requestItem = new RequestItem
            {
                Item = item,
                RequestedQty = Convert.ToInt32(qty)

            };

            List<RequestItem> session = (List<RequestItem>)Session["rForm"];
            if (session == null)
            {
                session = new List<RequestItem>();
            }

            session.Add(requestItem);

            Session["rForm"] = session;
            return RedirectToAction("Catalogue");
        }


        //Display the requisition form from Database
        public ActionResult ViewRequisitionForm()
        {

            List<RequestItem> requestItemList = (List<RequestItem>)Session["rForm"];
            List<RequestItem> NewRequestItemList = new List<RequestItem>();

            foreach (var requestItem in requestItemList)
            {
                RequestItem item = new RequestItem();
                item.Item = db.Items.SingleOrDefault(m => m.ItemNumber == requestItem.Item.ItemNumber);
                item.RequestedQty = requestItem.RequestedQty;

                NewRequestItemList.Add(item);

                //add in new list

            }
            RequisitionFormViewModel requisitionFormViewModel = new RequisitionFormViewModel();

            requisitionFormViewModel.RequestItems = NewRequestItemList;
            return View(requisitionFormViewModel);
        }


        //Update in Database 
        public ActionResult updateRequisitionForm()
        {
            List<RequestItem> requestItemList = (List<RequestItem>)Session["rForm"];
            List<RequestItem> NewRequestItemList = new List<RequestItem>();

            foreach (var requestItem in requestItemList)
            {
                RequestItem item = new RequestItem();
                item.Item = db.Items.SingleOrDefault(m => m.ItemNumber == requestItem.Item.ItemNumber);
                item.RequestedQty = requestItem.RequestedQty;

                NewRequestItemList.Add(item);
            }

            //Update Two Table RequestItem with Item and Quantity
            db.RequestItems.AddRange(NewRequestItemList);
            EmployeeRequestForm employeeRequestForm = new EmployeeRequestForm();
            employeeRequestForm.EmployeeRequestFormstatus = "Pending";
            //session User
            string username = Session["loginUserName"].ToString();
            var sfg = db.Dept_Staffs.SingleOrDefault(m => m.Username == username);
            employeeRequestForm.DeptStaff = db.Dept_Staffs.SingleOrDefault(m => m.Username == username);
            employeeRequestForm.RequestItems = NewRequestItemList;
            employeeRequestForm.DeptId = sfg.DeptId;
            db.EmployeeRequestForms.Add(employeeRequestForm);


            db.SaveChanges();
            Session["rForm"] = null;

            return RedirectToAction("Catalogue", "Item");

        }
        // remove item 
        public ActionResult RemoveItem(int itemId)

        {
            List<RequestItem> requestItemList = (List<RequestItem>)Session["rForm"];


            foreach (var item in requestItemList)
            {
                if (item.Item.Id == itemId)
                {
                    requestItemList.Remove(item);
                    break;
                }
            }


            Session["rForm"] = requestItemList;

            return RedirectToAction("/ViewRequisitionForm");

        }

        public ActionResult DisplayRequests()
        {
            var LoggedInUserRep = (Dept_Staff)Session["loginUser"];
            var reqs = db.EmployeeRequestForms.Include(c => c.DeptStaff).Where(m => m.EmployeeRequestFormstatus == "Approved" && m.DeptId == LoggedInUserRep.DeptId).ToList();
            return View(reqs);
        }

        public ActionResult DisplayRequestedItems(int id)
        {
            var reqs = db.EmployeeRequestForms.Include(i => i.RequestItems.Select(it => it.Item))
                .SingleOrDefault(m => m.EmployeeRequestFormId == id);
            return View(reqs);
        }
        [HttpPost]
        public ActionResult ConsolidateRequests()
        {
            var LoggedInUserRep = (Dept_Staff)Session["loginUser"];
            daoObj = new ConsolidatedRequestsService();
            var collections = db.CollectionPoints.ToList();
            var depts = db.Departments.ToList();
            var m = new ConsolidatedReqForm
            {
                dictionary = daoObj.ConsolidateIndividualRequests(LoggedInUserRep.DeptId),
                Collectionpoints = collections,
                Department = depts,

            };
            return View(m);
        }

        [HttpPost]
        public ActionResult UpdateCollectionPoint(FormCollection form)
        {
            var user = (Dept_Staff)Session["loginUser"];
            deptService = new DepartmentService();
            deptService.RetrieveDeptId(user, form);

            return RedirectToAction("DisplayRequests", "Item");
        }



        //Email
        public JsonResult SendMailToUser()
        {

            bool result = false;

            result = SendEmail("storeclerkteam4@gmail.com", "Email From Department",
                "<p>Dear Clerk,<br/>Requisition List has been submitted from the Department." +
                "<br>Regards,<br/></p>");

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
        //Interim
        public ViewResult PendingForApproval()
        {
            // string username = Session["loginUserName"].ToString();
            var pendingForApproval = db.EmployeeRequestForms.Include(c => c.DeptStaff).Where(m => m.EmployeeRequestFormstatus == "Pending").ToList();
            return View(pendingForApproval);
        }

        public ActionResult RequestDetails(int id)
        {
            var details = db.EmployeeRequestForms.Include(i => i.RequestItems.Select(it => it.Item))
                .SingleOrDefault(m => m.EmployeeRequestFormId == id);


            return View(details);
        }
        public ActionResult Approve(int id)
        {

            var sts = db.EmployeeRequestForms.SingleOrDefault(m => m.EmployeeRequestFormId == id);
            sts.EmployeeRequestFormstatus = "Approved";
            db.SaveChanges();

            return RedirectToAction("PendingForApproval", "Item");
        }

        public ActionResult Reject(int id)
        {

            var sts = db.EmployeeRequestForms.SingleOrDefault(m => m.EmployeeRequestFormId == id);
            sts.EmployeeRequestFormstatus = "Rejected";
            db.SaveChanges();

            return RedirectToAction("PendingForApproval", "Item");
        }

        public static string OTPCharacters()
        {
            string OTPLength = "6";

            string NewCharacters = "";

            //This one tells you which characters are allowed in this new password
            string allowedChars = "";
            //Here Specify your OTP Characters
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            //If you Need more secure OTP then uncomment Below Lines 
            // allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";        
            // allowedChars += "~,!,@,#,$,%,^,&,*,+,?";



            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);

            string IDString = "";
            string temp = "";

            //utilize the "random" class
            Random rand = new Random();


            for (int i = 0; i < Convert.ToInt32(OTPLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewCharacters = IDString;
            }

            return NewCharacters;
        }

        public static string OTPGenerator(string uniqueIdentity, string uniqueCustomerIdentity)
        {
            int length = 6;
            string oneTimePassword = "";
            DateTime dateTime = DateTime.Now;
            string _strParsedReqNo = dateTime.Day.ToString();
            _strParsedReqNo = _strParsedReqNo + dateTime.Month.ToString();
            _strParsedReqNo = _strParsedReqNo + dateTime.Year.ToString();
            _strParsedReqNo = _strParsedReqNo + dateTime.Hour.ToString();
            _strParsedReqNo = _strParsedReqNo + dateTime.Minute.ToString();
            _strParsedReqNo = _strParsedReqNo + dateTime.Second.ToString();
            _strParsedReqNo = _strParsedReqNo + dateTime.Millisecond.ToString();
            _strParsedReqNo = _strParsedReqNo + uniqueCustomerIdentity;


            _strParsedReqNo = uniqueIdentity + uniqueCustomerIdentity;
            using (MD5 md5 = MD5.Create())
            {
                byte[] _reqByte = md5.ComputeHash(Encoding.UTF8.GetBytes(_strParsedReqNo));
                //convert byte array to integer.
                int _parsedReqNo = BitConverter.ToInt32(_reqByte, 0);
                string _strParsedReqId = Math.Abs(_parsedReqNo).ToString();
                if (_strParsedReqId.Length < 9)
                {
                    StringBuilder sb = new StringBuilder(_strParsedReqId);
                    for (int k = 0; k < (9 - _strParsedReqId.Length); k++)
                    {
                        sb.Insert(0, '0');
                    }

                    _strParsedReqId = sb.ToString();
                }

                oneTimePassword = _strParsedReqId;
            }

            if (oneTimePassword.Length >= length)
            {
                oneTimePassword = oneTimePassword.Substring(0, length);
            }

            return oneTimePassword;
        }

        public ActionResult SendOtp()
        {
            Random rng = new Random();
            //Fetching OTP Characters
            string OtpCharacters = OTPCharacters();
            string otp = OTPGenerator(OtpCharacters, rng.Next(10).ToString());
            string msg = "Your Otp is:" + otp;
            Session["OTP"] = otp;
            SendEmail("deptrepresentativeteam4@gmail.com", "OTP", msg);
            return RedirectToAction("AcknowledgementApproval", "Item");
        }


        public ActionResult ValidOtp(string inputOtp)
        {
            var dep = (Dept_Staff)Session["loginUser"];
            var otp = Session["OTP"] as string;
            string userOtp = inputOtp;
            if (userOtp == otp)
            {
                var dls = db.DisbursementLists.SingleOrDefault(f => f.DeptId == dep.DeptId);

                dls.Status = "Approved";
                db.SaveChanges();
                return RedirectToAction("Catalogue");
            }
            else
            {
                var dls = db.DisbursementLists.SingleOrDefault(f => f.DeptId == dep.DeptId);
                dls.Status = "Rejected";
                db.SaveChanges();
                return RedirectToAction("Catalogue");

            }

        }
        public ActionResult AcknowledgementApproval()
        {
            ackService = new AcknowledgeService();
            int userid = Convert.ToInt32(Session["loginUserId"]);
            var reqs = ackService.retrieveConsolidatedReqs(userid);
            return View(reqs);
        }
        public JsonResult SendMailToClerk()
        {

            bool result = false;

            result = SendEmail("storeclerkteam4@gmail.com", "Email From Department",
                "<p>Dear Clerk,<br/>Acknowledgement is approved by the Department Representative." +
                "<br>Regards,<br/></p>");

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
