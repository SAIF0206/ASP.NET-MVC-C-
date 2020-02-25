using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_Project.Models.Departments
{
    public class ConsolidatedRequisition
    {
        [Key]
        public int ConsolidatedRequisitionId { get; set; }
        [ForeignKey("Department")]
        public int deptId { get; set; }
        public Department Department { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime RequestedDate { get; set; }

        public ICollection<CollectionOfRequestedItems> CollectionOfReq { get; set; }
        public string ConsolidatedRequisitionStatus { get; set; }

        [ForeignKey("CollectionPoint")]
        public int CollectionPointId { get; set; }
        public CollectionPoint CollectionPoint { get; set; }



    }
}
