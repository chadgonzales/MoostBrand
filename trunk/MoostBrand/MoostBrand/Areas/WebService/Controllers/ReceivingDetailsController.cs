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
    public class ReceivingDetailsController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/ReceivingDetails
        public IEnumerable<ReceivingDetail> Get()
        {
            return db.ReceivingDetails.Where(st => st.IsSync == false);
        }

        // POST: api/RecReceivingDetailseiving
        public HttpResponseMessage Post(ReceivingDetail receiving)
        {
            try
            {
                var r = db.ReceivingDetails.Find(receiving.ID);

                if (r != null)
                {
                    db.Entry(receiving).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.ReceivingDetails.Add(receiving);
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
