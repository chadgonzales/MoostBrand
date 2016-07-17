using MoostBrand.Areas.WebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MoostBrand.Areas.WebService.Controllers
{
    public class RequisitionDetailsController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/RequisitionDetails
        public IEnumerable<RequisitionDetail> Get()
        {
            return db.RequisitionDetails.Where(st => st.IsSync == false);
        }

        // POST: api/RequisitionDetails
        public HttpResponseMessage Post(RequisitionDetail requisition)
        {
            try
            {
                var r = db.RequisitionDetails.Find(requisition.ID);

                if (r != null)
                {
                    db.Entry(requisition).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.RequisitionDetails.Add(requisition);
                }

                db.SaveChanges();

                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                    "<strong>ok</strong>",
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
                    "<strong>failed</strong>",
                    Encoding.UTF8,
                    "text/html"
                )
            };
        }
    }
}
