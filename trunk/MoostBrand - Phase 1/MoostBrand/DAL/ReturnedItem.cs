namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReturnedItem
    {
        public int ID { get; set; }

        public int? ReturnID { get; set; }

        public int? ItemID { get; set; }

        public int? OldQuantity { get; set; }

        public int? NewQuantity { get; set; }
        public int? Pullout { get; set; }

        public string Image { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public virtual Return Return { get; set; }

        public string GetItem
        {
            get
            {
                StockAdjustmentRepository stockadRepo = new StockAdjustmentRepository();

                return stockadRepo.GetInventoryDescription(ItemID.Value);

            }
        }

    }
}
