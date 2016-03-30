namespace MoostBrand.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public int? BrandID { get; set; }

        [Required]
        [Display(Name = "Unit of Measurement")]
        public int? UnitOfMeasurementID { get; set; }

        [Required]
        public int? Quantity { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int? LocationID { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Location Location { get; set; }

        public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
    }
}
