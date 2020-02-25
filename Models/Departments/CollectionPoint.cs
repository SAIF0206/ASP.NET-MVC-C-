using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Project.Models.Departments
{
    public class CollectionPoint
    {
        [Key]
        public int CollectionPointId { get; set; }
        public String CollectionPlace { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? CollectionTime { get; set; }

    }
}