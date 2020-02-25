using AD_Project.DbContext;
using AD_Project.Models.Departments;
using AD_Project.Models.Store;
using AD_Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

/*
    MOHD SAIF ANSARI
*/

namespace AD_Project.Controllers
{
    [Authorize(Roles = "4,7")]

    public class EmployeeController : Controller
    {
        private ApplicationDbContext dbcontext;

        public EmployeeController()
        {
            dbcontext = new ApplicationDbContext();
        }

        //Dispose DbContext
        protected override void Dispose(bool disposing)
        {
            dbcontext.Dispose();
        }
        // GET: User
        public ActionResult DashBoard()
        {
            return View();
        }


        public ActionResult Catalogue(string query)
        {
            List<Item> items = dbcontext.Items.ToList();
            List<Item> newItems = new List<Item>();
            if (query != null)
            {
                var stationery = items.Where(x => x.ItemDesc.ToLower().Contains(query.ToLower())).ToList();
                return View(stationery);
            }
            else
            {
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
            var item = dbcontext.Items.Find(itemId);
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

        public ActionResult ViewRequisitionForm()
        {

            List<RequestItem> requestItemList = (List<RequestItem>)Session["rForm"];
            List<RequestItem> NewRequestItemList = new List<RequestItem>();


            foreach (var requestItem in requestItemList)
            {
                RequestItem item = new RequestItem();
                item.Item = dbcontext.Items.SingleOrDefault(m => m.ItemNumber == requestItem.Item.ItemNumber);
                item.RequestedQty = requestItem.RequestedQty;

                NewRequestItemList.Add(item);

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
                item.Item = dbcontext.Items.SingleOrDefault(m => m.ItemNumber == requestItem.Item.ItemNumber);
                item.RequestedQty = requestItem.RequestedQty;

                NewRequestItemList.Add(item);
            }

            //Update Two Table RequestItem with Item and Quantity
            dbcontext.RequestItems.AddRange(NewRequestItemList);
            EmployeeRequestForm employeeRequestForm = new EmployeeRequestForm();
            employeeRequestForm.EmployeeRequestFormstatus = "Pending";
            //session User
            string username = Session["loginUserName"].ToString();
            var sfg = dbcontext.Dept_Staffs.SingleOrDefault(m => m.Username == username);
            employeeRequestForm.DeptStaff = dbcontext.Dept_Staffs.SingleOrDefault(m => m.Username == username);
            employeeRequestForm.RequestItems = NewRequestItemList;
            employeeRequestForm.DeptId = sfg.DeptId;
            dbcontext.EmployeeRequestForms.Add(employeeRequestForm);


            dbcontext.SaveChanges();
            Session["rForm"] = null;

            return RedirectToAction("Catalogue");

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


        /*
        Keerthana
         */
        //Interim
        public ViewResult PendingForApproval()
        {
            var LoggedInUserhead = (Dept_Staff)Session["loginUser"];
            var pendingForApproval = dbcontext.EmployeeRequestForms.Include(c => c.DeptStaff).Where(m => m.EmployeeRequestFormstatus == "Pending" && m.DeptId == LoggedInUserhead.DeptId).ToList();
            return View(pendingForApproval);
        }

        public ActionResult RequestDetails(int id)
        {
            var details = dbcontext.EmployeeRequestForms.Include(i => i.RequestItems.Select(it => it.Item))
                .SingleOrDefault(m => m.EmployeeRequestFormId == id);


            return View(details);
        }
        public ActionResult Approve(int id)
        {

            var sts = dbcontext.EmployeeRequestForms.SingleOrDefault(m => m.EmployeeRequestFormId == id);
            sts.EmployeeRequestFormstatus = "Approved";
            dbcontext.SaveChanges();

            return RedirectToAction("PendingForApproval", "Employee");
        }

        public ActionResult Reject(int id)
        {

            var sts = dbcontext.EmployeeRequestForms.SingleOrDefault(m => m.EmployeeRequestFormId == id);
            sts.EmployeeRequestFormstatus = "Rejected";
            dbcontext.SaveChanges();

            return RedirectToAction("PendingForApproval", "Employee");
        }



    }
}
