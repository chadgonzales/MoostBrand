namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web;

    public partial class Receiving
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Receiving()
        {
            ReceivingDetails = new HashSet<ReceivingDetail>();
            StockAllocations = new HashSet<StockAllocation>();
        }

        public int ID { get; set; }

        public int LocationID { get; set; }

        public int ReceivingTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceivingID { get; set; }

        public int StockTransferID { get; set; }

        public DateTime ReceivingDate { get; set; }

        [StringLength(50)]
        public string ReceivingTime { get; set; }

        public int? EncodedBy { get; set; }

        public int? CheckedBy { get; set; }

        public int ReceivedBy { get; set; }

        [StringLength(50)]
        public string PONumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Vendor Invoice Number")]
        public string DRNumber { get; set; }

        public string InvoiceReference { get; set; }

        [StringLength(50)]
        [Display(Name = "Sales Invoice Number")]
        public string InvoiceNumber { get; set; }

        [StringLength(50)]
        public string VesselNumber { get; set; }

        [StringLength(50)]
        public string VoyageNumber { get; set; }

        [StringLength(100)]
        public string WaybillNumber { get; set; }

        [StringLength(100)]
        public string ShippingLine { get; set; }

        [StringLength(100)]
        public string Forwarder { get; set; }

        [StringLength(50)]
        public string VanNumber { get; set; }

        public int? ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        [StringLength(100)]
        public string ReceivingLocation { get; set; }

        public string Image { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual Employee Employee2 { get; set; }

        public virtual Employee Employee3 { get; set; }

        public virtual Location Location { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingDetail> ReceivingDetails { get; set; }

        public virtual ReceivingType ReceivingType { get; set; }

        public virtual StockTransfer StockTransfer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAllocation> StockAllocations { get; set; }

        [Display(Name = "Upload Image")]
        [ValidateImage]
        [NotMapped]
        public HttpPostedFileBase Img { get; set; }

        public class ValidateImageAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                int MaxContentLength = 1024 * 1024 * 10; //10 MB
                string[] AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" };

                var file = value as HttpPostedFileBase;

                if (file == null)
                {
                    return true;
                }
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()))
                {
                    ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "MB";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
