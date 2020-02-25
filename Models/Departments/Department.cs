using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Departments
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }
        public String DeptName { get; set; }
        public String DeptHead { get; set; }
        public String DeptRep { get; set; }
        public CollectionPoint CollectionPoint { get; set; }
        public ICollection<Dept_Staff> DeptStaff { get; set; }

    }
}