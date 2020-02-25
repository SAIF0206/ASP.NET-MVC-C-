using AD_Project.Models.Departments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class DeptRequestedItems
    {
        [Key]
        public int Id { get; set; }

        public Item Item { get; set; }
        public int itemId { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
        public int DeptId { get; set; }
        public int DeptRequestedQtyForEachItem { get; set; }
        public int ActualDeliveredQtyForEachItem { get; set; }
    }
}