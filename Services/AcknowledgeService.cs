using AD_Project.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AD_Project.Models.Departments;
using AD_Project.ViewModel;

namespace AD_Project.DAO
{
    public class AcknowledgeService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public AcknowledgeViewModel retrieveConsolidatedReqs(int userid)
        {
            //session
            
            var depart = db.Dept_Staffs.SingleOrDefault(d => d.UserId == userid);
            int departId = depart.DeptId;

            var consolidatedQty = db.ConsolidatedRequisitions.
                Include(k => k.Department).Include(l => l.CollectionPoint).
                Include(s => s.CollectionOfReq.Select(it => it.Item)).
                Where(w => w.deptId == departId && w.ConsolidatedRequisitionStatus == "Approved").ToList();

            var disburseItems = db.DisbursementLists
                .Include(k => k.DisburseItems)
                .Where(r => r.Department.DeptId == departId && r.Status == "pending").ToList();

            AcknowledgeViewModel ackViewModel = new AcknowledgeViewModel
            {
                consolidatedRequisition = consolidatedQty,
                disbursementList = disburseItems,

            };
            return ackViewModel;
        }
    }
}