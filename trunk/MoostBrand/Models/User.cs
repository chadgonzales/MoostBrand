namespace MoostBrand.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public int ID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int UserTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Designation { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual UserType UserType { get; set; }
    }
}
