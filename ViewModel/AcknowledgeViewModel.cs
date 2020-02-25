using AD_Project.Models.Departments;
using AD_Project.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Project.ViewModel
{
    public class AcknowledgeViewModel
    {
        public List<ConsolidatedRequisition> consolidatedRequisition { get; set; }
    //    public List<CollectionOfRequestedItems> collectionOfRequested { get; set; }
        public List<DisbursementList> disbursementList { get; set; }
      //  public List<DisburseItem> disburseItem { get; set; }

    }
}