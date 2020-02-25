using AD_Project.DbContext;
using AD_Project.Models.Departments;
using AD_Project.ViewModel;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

/*
 Mohd Saif Ansari
     */
namespace AD_Project.Controllers.api
{
    public class RepController : ApiController
    {
        private ApplicationDbContext _context;

        public RepController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public IHttpActionResult CollectionPoint()
        {
            var consolidatedRequisitions = _context.ConsolidatedRequisitions.ToList();
            var CollectionPoint = _context.CollectionPoints.ToList();
            var reqs = new CollectionPointViewModel
            {
                CollectionPoint = CollectionPoint,
                consolidatedRequisition = consolidatedRequisitions,
            };

            return Ok(reqs);
        }


        [HttpPost]
        public IHttpActionResult updateCollectionPoint(ChangeCollectionPoint changeCollectionPoints)
        {
            string place = changeCollectionPoints.CollectionPlace;
            var collectionPnts = _context.CollectionPoints.SingleOrDefault(g => g.CollectionPlace == place);
            int colId = collectionPnts.CollectionPointId;

            ConsolidatedRequisition consolidated = _context.ConsolidatedRequisitions.Include(i => i.CollectionOfReq).SingleOrDefault(m => m.deptId == changeCollectionPoints.DeptId);
            consolidated.CollectionPoint = collectionPnts;
            _context.SaveChanges();
            return Ok();
        }




    }
}
