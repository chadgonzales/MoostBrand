namespace MoostBrand.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            Items = new HashSet<Item>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Location Type")]
        public int? LocationTypeID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }

        public virtual LocationType LocationType { get; set; }
    }
}
