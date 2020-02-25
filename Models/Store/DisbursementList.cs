using AD_Project.Models.Departments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class DisbursementList
    {
        [Key]
        public int DisbursementListId { get; set; }
        public DateTime DisbursementListDate { get; set; }
        public Department Department { get; set; }
        public int DeptId { get; set; }
        public String Status { get; set; }
        public ICollection<DisburseItem> DisburseItems { get; set; }

    }
}