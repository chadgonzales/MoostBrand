namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            RequisitionDetails = new HashSet<RequisitionDetail>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Barcode { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public int? CategoryID { get; set; }

        public int? SubCategoryID { get; set; }

        public int? BrandID { get; set; }

        public int? ColorID { get; set; }

        public int? SizeID { get; set; }

        public int? UnitOfMeasurementID { get; set; }

        public int? ReOrderLevel { get; set; }

        public int? MinimumStock { get; set; }

        public int? MaximumStock { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Category Category { get; set; }

        public virtual Color Color { get; set; }

        public virtual Size Size { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequisitionDetail> RequisitionDetails { get; set; }
    }
}
