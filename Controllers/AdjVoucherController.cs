using AD_Project.DbContext;
using AD_Project.Models.Store;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;


// Adhiyaman
namespace AD_Project.Controllers
{
    [Authorize(Roles = "1")]
    public class AdjVoucherController : Controller
    {
        ApplicationDbContext Db = new ApplicationDbContext();
        // GET: AdjVoucher
        public ActionResult create()
        {

            return View();
        }


        [HttpPost]
        public ActionResult save(AdjustedItem adj, FormCollection form)
        {
            int ItemIdInForm = int.Parse(form["ItemId"]);
            var reqs = Db.Items.SingleOrDefault(i => i.Id == ItemIdInForm);
            if (reqs == null)
            {
                Item item = new Item();
                adj.Item = item;
                Db.AdjustedItems.Add(adj);
                Db.SaveChanges();

            }
            else
            {
                adj.ItemId = int.Parse(form["ItemId"]);
                adj.status = "Pending";
                Db.AdjustedItems.Add(adj);
                Db.SaveChanges();
            }

            return RedirectToAction("Home", "Adjvoucher");
        }

        public ActionResult Home()
        {

            var voucher = Db.AdjustedItems.Include(i => i.Item).Where(h => h.status == "Pending").ToList();
            return View(voucher);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var voucher = Db.AdjustedItems.Include(i => i.Item).SingleOrDefault(i => i.AdjustedItemId == id);
            return View(voucher);
        }


        [HttpPost]
        public ActionResult Edit(AdjustedItem productObj)
        {
            var product = Db.AdjustedItems.Include(i => i.Item).SingleOrDefault(i => i.AdjustedItemId == productObj.AdjustedItemId);

            product.Item.ItemNumber = productObj.Item.ItemNumber;
            product.AdjustedQty = productObj.AdjustedQty;
            product.AdjustmentReason = productObj.AdjustmentReason;
            Db.SaveChanges();
            return RedirectToAction("Home");
        }

        public ActionResult Delete(int id)
        {
            var product = Db.AdjustedItems.Include(i => i.Item).Where(i => i.AdjustedItemId == id).FirstOrDefault();
            Db.AdjustedItems.Remove(product);
            Db.SaveChanges();
            return RedirectToAction("Home");
        }
    }
}
