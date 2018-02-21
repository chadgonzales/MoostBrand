namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vendor()
        {
            Requisitions = new HashSet<Requisition>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string GeneralName { get; set; }

        [StringLength(50)]
        public string Attn { get; set; }

        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string ContactNo { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        public string ShippingLine { get; set; }

        public string AcctDetails { get; set; }

        [StringLength(50)]
        public string FaxNumber { get; set; }

        public string Remarks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition> Requisitions { get; set; }
    }
}
