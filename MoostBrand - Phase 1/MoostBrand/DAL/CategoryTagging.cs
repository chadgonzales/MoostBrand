namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CategoryTagging
    {
        public int ID { get; set; }

        public int ItemID { get; set; }

        public int CategoryID { get; set; }

        public int SubCategoryID { get; set; }

        public virtual Category Category { get; set; }

        public virtual Item Item { get; set; }

        public virtual SubCategory SubCategory { get; set; }
    }
}
