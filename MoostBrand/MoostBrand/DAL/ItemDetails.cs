using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL
{
    public class ItemDetails
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int? Quantity { get; set; }
        public decimal? Cost { get; set; }
        public int? ItemID { get; set; }
        public decimal? WeightedAverageCost { get; set; }
        public virtual Item Item { get; set; }
    }
}