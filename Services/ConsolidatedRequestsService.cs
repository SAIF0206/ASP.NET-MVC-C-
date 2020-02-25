using AD_Project.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AD_Project.DAO
{

    public class ConsolidatedRequestsService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Dictionary<int, int> ConsolidateIndividualRequests(int deptId)
        {
            var depts = db.Departments.ToList();
            var collections = db.CollectionPoints.ToList();

            var reqs = db.EmployeeRequestForms.Include(i => i.RequestItems.Select(it => it.Item)).Where(n => n.EmployeeRequestFormstatus == "Approved" && n.DeptId == deptId);
            Dictionary<int, int> cart = new Dictionary<int, int>();
            /*  foreach(var e in reqs)
              {
                  e.EmployeeRequestFormstatus = "Consolidated";
              }*/
            foreach (var r in reqs)
            {
                foreach (var i in r.RequestItems)
                {
                    if (cart.ContainsKey(i.Item.Id))
                    {
                        int val = cart[i.Item.Id];
                        val += i.RequestedQty;
                        cart[i.Item.Id] = val;
                    }
                    else
                    {
                        cart.Add(i.Item.Id, i.RequestedQty);
                    }

                }

            }
            //   db.SaveChanges();
            return cart;
        }

    }
}