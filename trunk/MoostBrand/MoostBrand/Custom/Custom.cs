using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class Req
    {
        public Requisition Requisitions { get; set; }

    }

    public class ReqDetails
    {
        public RequisitionDetail RequisitionDetails { get; set; }

    }



}