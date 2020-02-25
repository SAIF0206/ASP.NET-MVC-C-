using AD_Project.Models.Store;
using System.Collections.Generic;

namespace AD_Project.ViewModel
{
    public class RetrievalFormViewModel
    {
        public List<DeptRequestedItems> deptRequestedItems { get; set; }
        public List<StoreRetrievalList> storeRetrievalLists { get; set; }

    }
}
