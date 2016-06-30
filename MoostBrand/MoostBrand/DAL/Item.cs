//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.RequisitionDetails = new HashSet<RequisitionDetail>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> SubCategoryID { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> ColorID { get; set; }
        public Nullable<int> SizeID { get; set; }
        public Nullable<int> UnitOfMeasurementID { get; set; }
        public Nullable<int> ReOrderLevel { get; set; }
        public Nullable<int> MinimumStock { get; set; }
        public Nullable<int> MaximumStock { get; set; }
    
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
