namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reservation
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        public int? ItemID { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int? QuantityUnit { get; set; }

        public int? ReservationTypeID { get; set; }

        public int? QuantityReserved { get; set; }

        public int? IsPaid { get; set; }

        public int? EmployeeInCharge { get; set; }

        public int? EncodedBy{ get; set; }

        public DateTime? DueDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
