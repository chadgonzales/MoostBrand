using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoostBrand.DAL
{
    public class Operator
    {
        public int OperatorID { get; set; }

        public int? StockTransferID { get; set; }

        [Required]
        public string Name { get; set; }

        public int? DeletedOperator { get; set; }

        public virtual StockTransfer StockTransfer { get; set; }

    }
}