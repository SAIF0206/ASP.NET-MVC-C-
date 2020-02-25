using AD_Project.DbContext;
using AD_Project.Models.Departments;
using AD_Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AD_Project.DAO
{
    public class DepartmentService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void RetrieveDeptId(Dept_Staff user, FormCollection form)
        {
            int deptId = user.DeptId;
          //  int deptIdInForm = int.Parse(form["Department"]);
            var reqs = db.Departments.Select(i => i.DeptId == deptId);
            if (reqs == null)
            {
                Dictionary<int, int> dictionary = new Dictionary<int, int>();
                ConsolidatedRequestsService daoObj = new ConsolidatedRequestsService();
                dictionary = daoObj.ConsolidateIndividualRequests(deptId);
                List<CollectionOfRequestedItems> CollectionOfReqList =
                  new List<CollectionOfRequestedItems>();
                foreach (var i in dictionary)
                {
                    CollectionOfRequestedItems collectionOfReq = new CollectionOfRequestedItems();
                    collectionOfReq.ItemId = i.Key;
                    collectionOfReq.TotalRequestedQty = i.Value;
                    db.collectionOfRequestedItems.Add(collectionOfReq);
                    CollectionOfReqList.Add(collectionOfReq);
                }
                ConsolidatedRequisition cons = new ConsolidatedRequisition();

                CollectionPoint coll = new CollectionPoint();
                coll.CollectionPointId = int.Parse(form["CollectionPoint"]);

                
                Department dept = new Department();
                dept.DeptId = deptId;


                cons.CollectionPoint = coll;
                cons.Department = dept;
                cons.CollectionOfReq = CollectionOfReqList;
                cons.ConsolidatedRequisitionStatus = "Approved";
                cons.RequestedDate = DateTime.Now;
                db.ConsolidatedRequisitions.Add(cons);
                var empReqs = db.EmployeeRequestForms.Include(i => i.RequestItems.Select(it => it.Item)).Where(n => n.EmployeeRequestFormstatus == "Approved");

                  foreach(var e in empReqs)
                  {
                      e.EmployeeRequestFormstatus = "Consolidated";
                  }
                db.SaveChanges();

            } else
            {
                Dictionary<int, int> dictionary = new Dictionary<int, int>();
                ConsolidatedRequestsService daoObj = new ConsolidatedRequestsService();
                dictionary = daoObj.ConsolidateIndividualRequests(deptId);
                List<CollectionOfRequestedItems> CollectionOfReqList =
                  new List<CollectionOfRequestedItems>();
                foreach (var i in dictionary)
                {
                    CollectionOfRequestedItems collectionOfReq = new CollectionOfRequestedItems();
                    collectionOfReq.ItemId = i.Key;
                    collectionOfReq.TotalRequestedQty = i.Value;
                    db.collectionOfRequestedItems.Add(collectionOfReq);
                    CollectionOfReqList.Add(collectionOfReq);
                }
                ConsolidatedRequisition cons = new ConsolidatedRequisition();
                cons.CollectionPointId = int.Parse(form["CollectionPoint"]);


            //    int deptId = user.DeptId;
                cons.deptId = deptId;

                cons.CollectionOfReq = CollectionOfReqList;
                cons.ConsolidatedRequisitionStatus = "Approved";
                cons.RequestedDate = DateTime.Now;
                db.ConsolidatedRequisitions.Add(cons);

                var empReqs = db.EmployeeRequestForms.Include(i => i.RequestItems.Select(it => it.Item)).Where(n => n.EmployeeRequestFormstatus == "Approved");

                foreach (var e in empReqs)
                {
                    e.EmployeeRequestFormstatus = "Consolidated";
                }
                db.SaveChanges();


            }
        }
    }
}