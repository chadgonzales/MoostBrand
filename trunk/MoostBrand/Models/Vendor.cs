namespace MoostBrand.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vendor
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string VendorName { get; set; }

        [Required]
        [StringLength(50)]
        public string VendorGeneralName { get; set; }

        [Required]
        [StringLength(50)]
        public string Attn { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }
    }
}
