namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserAccess
    {
        public UserAccess()
        {
        }

        public UserAccess(int i, string moduleName)
        {
            ModuleID = i;
            ModuleName = moduleName;
        }

        [NotMapped]
        public string ModuleName { get; set; }

        public int ID { get; set; }

        public int? EmployeeID { get; set; }

        public int? ModuleID { get; set; }

        public bool CanView { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanRequest { get; set; }

        public bool CanDecide { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Module Module { get; set; }
    }
}
