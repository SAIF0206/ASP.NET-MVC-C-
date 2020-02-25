using AD_Project.Models.Departments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Store
{
    public class RetrievalList
    {
        [Key]
        public int RetrievalListId { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
        public int DeptId { get; set; }
        public Item Item { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public int RequestedQty { get; set; }
        public int ActualQty { get; set; }
       
    }
}