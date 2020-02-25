using AD_Project.DbContext;
using AD_Project.Models.Departments;
using AD_Project.ViewModel;
using System.Linq;
using System.Web.Http;

/*
Mohd Saif Ansari
 */

namespace AD_Project.Controllers.api
{
    public class DelegateController : ApiController
    {
        private ApplicationDbContext _context;


        public DelegateController()
        {
            _context = new ApplicationDbContext();

        }

        [HttpGet]
        public IHttpActionResult AssignDelegate()
        {

            var assign = new ListDept_Staff();
            assign.Dept_Staffs = _context.Dept_Staffs.ToList();


            return Ok(assign);

        }

        [HttpPost]
        public IHttpActionResult Assign(HeadDelegate headDelegate)
        {

            Dept_Staff sts = _context.Dept_Staffs.SingleOrDefault(m => m.Username == headDelegate.AssignedDept_Staff.Username);
            sts.Staff_Status = "Assigned";
            HeadDelegate del = new HeadDelegate
            {
                UserId = sts.UserId,
                StartDate = headDelegate.StartDate,
                EndDate = headDelegate.EndDate
            };
            _context.HeadDelegates.Add(del);
            _context.SaveChanges();
            return Ok();
        }

    }
}
