using AD_Project.DbContext;
using AD_Project.ViewModel;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

/*
Keerthana
 */
namespace AD_Project.Controllers.api
{
    public class DisbursementRepController : ApiController

    {
        public ApplicationDbContext _context;

        public DisbursementRepController()
        {
            _context = new ApplicationDbContext();
        }

        //Code here Keerthana
        [HttpGet]
        public IHttpActionResult DisbursementList()
        {
            // int deptid = 1;
            var disburseItems = _context.DisbursementLists.Include(h => h.DisburseItems).ToList();
            //  Where(w => w.DeptId == deptid && w.Status == "pending").ToList();
            var items = _context.Items.ToList();
            var disburseView = new DisburseAndroidViewModel();
            disburseView.Disbursements = disburseItems;
            disburseView.Items = items;
            return Ok(disburseView);
        }

        [HttpPost]
        public IHttpActionResult UpdateAcknowledgement()
        {
            /*
                        DibursementList dis = _context.DisbursementLists
                            .Include(k => k.DisburseItems)
                            .Where(r => r.Department.DeptId == deptId).ToList();
                        foreach (var r in disburseView.Disbursements)
                        {
                            int deptId = r.DeptId;
                            foreach(var h in r.DisburseItems)
                            {
                                int qty = h.DisburseQty;
                            }
                        }



                        dis.Status = "Delivered";
                        _context.SaveChanges();*/
            return Ok();
        }




    }
}
