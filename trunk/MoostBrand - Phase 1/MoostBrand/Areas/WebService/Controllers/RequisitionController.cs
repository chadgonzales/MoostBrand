using MoostBrand.Areas.WebService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MoostBrand.Areas.WebService.Controllers
{
    public class RequisitionController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/Requisition
        public IEnumerable<Requisition> Get()
        {
            return db.Requisitions.Where(st => st.IsSync == false);
        }

        // POST: api/Requisition
        public HttpResponseMessage Post(Requisition requisition)
        {
            try
            {
                requisition.IsSync = true;

                db.Requisitions.Add(requisition);
                db.SaveChanges();

                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                    "ok",
                    Encoding.UTF8,
                    "text/html"
                )
                };
            }
            catch
            {
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    "failed",
                    Encoding.UTF8,
                    "text/html"
                )
            };
        }
    }
}
