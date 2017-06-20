using System.Collections.Generic;

namespace MoostBrand.DAL
{
    public class Helper
    {
        public int HelperID { get; set; }

        public int? StockTransferID { get; set; }
        
        public string Name { get; set; }

        public int? DeletedHelper { get; set; }

        public virtual StockTransfer StockTransfer { get; set; }
    }
}