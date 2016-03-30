namespace MoostBrand.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SubCategory
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        public int? CategoryID { get; set; }

        [StringLength(100)]
        public string Picture { get; set; }

        public virtual Category Category { get; set; }
    }
}
