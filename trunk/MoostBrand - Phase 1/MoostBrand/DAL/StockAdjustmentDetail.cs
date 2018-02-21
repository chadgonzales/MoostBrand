namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockAdjustmentDetail
    {
        public int ID { get; set; }

        public int? StockAdjustmentID { get; set; }

        public int ItemID { get; set; }

        public int? OldQuantity { get; set; }

        [Required(ErrorMessage = "New Quantity is Required")]
        public int? NewQuantity { get; set; }

        [Required(ErrorMessage = "Variance is Required")]
        public int? Variance { get; set; }

        public string Notes { get; set; }

        public bool? IsSync { get; set; }

      
        public virtual StockAdjustment StockAdjustment { get; set; }

        public string GetItem
        {
            get
            {
                StockAdjustmentRepository stockadRepo = new StockAdjustmentRepository();

                return stockadRepo.GetInventoryDescription(ItemID);

            }
        }


    }
}
