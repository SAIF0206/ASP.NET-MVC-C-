using AD_Project.DbContext;
using AD_Project.ViewModel;
using System.Linq;
using System.Web.Http;

/*
Keerthana
 */

namespace AD_Project.Controllers.api
{
    public class AdjVoucherController : ApiController
    {
        private ApplicationDbContext _context;


        public AdjVoucherController()
        {
            _context = new ApplicationDbContext();

        }

        [HttpGet]
        public IHttpActionResult GetAdjVoucher()
        {
            var adjustedItem = _context.AdjustedItems.ToList();
            var Items = _context.Items.ToList();
            var Supp = _context.Suppliers_Prices.ToList();
            var adjView = new AdjVoucherViewModel
            {
                AdjustedItems = adjustedItem,
                Item = Items,
                SuppliersPrice = Supp,
            };
            return Ok(adjView);
        }
    }
}
