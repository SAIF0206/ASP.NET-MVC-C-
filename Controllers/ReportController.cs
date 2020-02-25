using AD_Project.DbContext;
using AD_Project.Models.Store;
using AD_Project.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

/*
Adhiyaman
 */

namespace AD_Project.Controllers
{
    [Authorize(Roles = "2,3")]
    public class ReportController : Controller
    {

        ApplicationDbContext Db = new ApplicationDbContext();
        // GET: Report
        public ActionResult Report()
        {

            List<DataPoint> dataPoints = new List<DataPoint>();

            var dbPoints = Db.Items.ToList();
            foreach (var item in dbPoints)
            {
                dataPoints.Add(new DataPoint(item.ItemDesc, item.CurrentQty));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            List<Models.Departments.Department> deptName = Db.Departments.ToList();
            var viewModel = new ReportViewModel
            {
                department = deptName
            };
            return View(viewModel);
        }

        public ActionResult TrendReport()
        {
            List<DataPoint> dataPoints2 = new List<DataPoint>();

            var dbPoints = Db.TrendAnalyses.ToList();
            foreach (var item in dbPoints)
            {
                dataPoints2.Add(new DataPoint(item.Date, item.Quantity));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints2);

            var viewModel = new ReportViewModel
            {

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ReportOfEachDept(ReportViewModel reportViewModel)
        {
            var dbPoint = Db.DisbursementLists.Include(s => s.DisburseItems).ToList();
            List<DataPoint> dataPoints1 = new List<DataPoint>();
            if (reportViewModel.selectedDeptId == 1)
            {


                foreach (var i in dbPoint)
                {
                    if (i.DeptId == reportViewModel.selectedDeptId)
                    {
                        foreach (var u in i.DisburseItems)
                        {
                            dataPoints1.Add(new DataPoint(u.ItemId.ToString(), u.DisburseQty));
                            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
                        }

                    }

                }
            }
            else if (reportViewModel.selectedDeptId == 2)
            {

                foreach (var i in dbPoint)
                {
                    if (i.DeptId == reportViewModel.selectedDeptId)
                    {
                        foreach (var u in i.DisburseItems)
                        {
                            dataPoints1.Add(new DataPoint(u.ItemId.ToString(), u.DisburseQty));
                            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
                        }

                    }

                }

            }
            else if (reportViewModel.selectedDeptId == 3)
            {

                foreach (var i in dbPoint)
                {
                    if (i.DeptId == reportViewModel.selectedDeptId)
                    {
                        foreach (var u in i.DisburseItems)
                        {
                            dataPoints1.Add(new DataPoint(u.ItemId.ToString(), u.DisburseQty));
                            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
                        }

                    }

                }

            }
            var viewModel = new ReportViewModel
            {
                disbursementList = dbPoint,

            };
            return View(viewModel);
        }



    }
}
