using AD_Project.Models.Departments;
using AD_Project.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Project.ViewModel
{
    public class DisbursementViewModel
    {
        public DisbursementList DisbursementList { get; set; }
        public List<Department> Departments { get; set; }
        public int SelectedDepId { get; set; }
    }
}