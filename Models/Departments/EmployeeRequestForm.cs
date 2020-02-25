using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Departments
{
    public class EmployeeRequestForm
    {
        [Key]
        public int EmployeeRequestFormId { get; set; }
        public Dept_Staff DeptStaff { get; set; }
        [ForeignKey("DeptStaff")]
        public int DeptStaffId { get; set; }
        public int DeptId { get; set; }
        public string EmployeeRequestFormstatus { get; set; }
        public ICollection<RequestItem> RequestItems { get; set; }


    }
}