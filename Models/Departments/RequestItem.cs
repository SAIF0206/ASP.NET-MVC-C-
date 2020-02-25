using AD_Project.Models.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Departments
{
    public class RequestItem
    {
        [Key]
        public int RequestItemId { get; set; }
        public Item Item { get; set; }
        public int RequestedQty { get; set; }
        [Column("EmployeeRequestFormId")]
        public EmployeeRequestForm EmployeeRequest { get; set; }
      /*  [ForeignKey("EmployeeRequest")]
        public int EmployeeRequestFormId { get; set; }*/
    }
}