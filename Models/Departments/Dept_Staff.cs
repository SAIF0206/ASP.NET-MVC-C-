using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Departments
{
    public class Dept_Staff : User
    {
        public Department Department { get; set; }
        [ForeignKey("Department")]
        public int DeptId { get; set; }

        public String Staff_Status { get; set; }

        public ICollection<EmployeeRequestForm> employeeRequestForms { get; set; }

    }
}