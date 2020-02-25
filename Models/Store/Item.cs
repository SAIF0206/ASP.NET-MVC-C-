using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD_Project.Models.Store
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string ItemNumber { get; set; }
        [StringLength(50)]
        public string ItemDesc { get; set; }
        public string ItemCategory { get; set; }
        public string UnitOfMeasure { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQty { get; set; }
        [ForeignKey("Suppliers_Prices")]
        public int? SupplierPriceId { get; set; }
        public ICollection<Suppliers_Price> Suppliers_Prices { get; set; }
        [ForeignKey("Stockdetails")]
        public int? StockDetailsId { get; set; }
        public ICollection<StockDetails> Stockdetails { get; set; }

        public int CurrentQty { get; set; }
    }
}
