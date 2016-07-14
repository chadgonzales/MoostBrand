namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public int ID { get; set; }

        public int EmployeeID { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public int UserTypeID { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        public int? LocationID { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Location Location { get; set; }

        public virtual UserType UserType { get; set; }
    }
}
