using AD_Project.DbContext;
using AD_Project.Models.Store;
using AD_Project.ViewModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AD_Project.Services
{
    public class DisplayRetrievalForm
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public RetrievalFormViewModel DisplayRetrieval()
        {
            var stockDetails = db.Stockdetails.Include(l => l.item).ToList();
            var reqs = db.ConsolidatedRequisitions.Include(i => i.CollectionOfReq.Select(it => it.Item))
                .Where(k => k.ConsolidatedRequisitionStatus == "Approved").ToList();

            List<int> ItemIdList = new List<int>();
            List<DeptRequestedItems> DeptList = new List<DeptRequestedItems>();
            List<StoreRetrievalList> StoreRetrievalList = new List<StoreRetrievalList>();

            foreach (var f in reqs)
            {


                foreach (var r in f.CollectionOfReq)
                {

                    if (ItemIdList.Contains(r.ItemId))
                    {
                        var StoreRetrieval = StoreRetrievalList.Find(m => m.ItemId == r.ItemId);

                        DeptRequestedItems deptRequested = new DeptRequestedItems();
                        ItemIdList.Add(r.ItemId);
                        deptRequested.DeptId = f.deptId;
                        deptRequested.itemId = r.ItemId;
                        deptRequested.DeptRequestedQtyForEachItem = r.TotalRequestedQty;
                        deptRequested.ActualDeliveredQtyForEachItem = 0;

                        StoreRetrieval.TotalNeededQty += r.TotalRequestedQty;

                        StoreRetrieval.DeptRequestedItems.Add(deptRequested);

                    }
                    else
                    {
                        var storeRetrieval = new StoreRetrievalList();

                        DeptRequestedItems deptRequested = new DeptRequestedItems();
                        ItemIdList.Add(r.ItemId);
                        deptRequested.DeptId = f.deptId;
                        deptRequested.itemId = r.ItemId;
                        deptRequested.DeptRequestedQtyForEachItem = r.TotalRequestedQty;
                        deptRequested.ActualDeliveredQtyForEachItem = 0;
                        db.DeptRequestedItems.Add(deptRequested);
                        db.SaveChanges();
                        DeptList.Add(deptRequested);

                        foreach (var item in stockDetails)
                        {
                            if (item.ItemId == r.ItemId)
                            {
                                storeRetrieval.StockQty = item.InventoryStockQty;
                            }
                        }

                        storeRetrieval.ItemId = r.ItemId;
                        storeRetrieval.TotalNeededQty = r.TotalRequestedQty;
                        storeRetrieval.DeptRequestedItems = DeptList;
                        StoreRetrievalList.Add(storeRetrieval);
                    }
                }


            }
            RetrievalFormViewModel retrievalFormView = new RetrievalFormViewModel
            {
                storeRetrievalLists = StoreRetrievalList,
                deptRequestedItems = DeptList,
            };
            db.SaveChanges();
            return retrievalFormView;
        }
    }
}
