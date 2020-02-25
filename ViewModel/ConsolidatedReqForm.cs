using AD_Project.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Project.ViewModel
{
    public class ConsolidatedReqForm
    {
        public Dictionary<int, int> dictionary { get; set; }
        public IEnumerable<CollectionPoint> Collectionpoints { get; set; }
        public IEnumerable<Department> Department { get; set; }
       public ConsolidatedList ConsolidatedReqFormodel { get; set; }
    }
}