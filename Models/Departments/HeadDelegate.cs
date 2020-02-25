using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Departments
{
    public class HeadDelegate
    {
        [Key]
        public int DelegateId { get; set; }
        [Required(ErrorMessage = "Startdate is required")]
        public string StartDate { get; set; }
        [Required(ErrorMessage = "Enddate is required")]
        public string EndDate { get; set; }
        public Dept_Staff AssignedDept_Staff { get; set; }
        [ForeignKey("AssignedDept_Staff")]
        public int UserId { get; set; }


    }
}