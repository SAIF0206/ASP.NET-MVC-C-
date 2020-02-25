using AD_Project.Models.Departments;
using AD_Project.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Project.ViewModel
{
    public class ReportViewModel
    {
        public List<DisbursementList> disbursementList { get; set; }
        public DataPoint dataPoint { get; set; }
        public List<Department> department { get; set; }
        public int selectedDeptId { get; set; }
        public TrendAnalysis trendAnalysis { get; set; }
    }
}