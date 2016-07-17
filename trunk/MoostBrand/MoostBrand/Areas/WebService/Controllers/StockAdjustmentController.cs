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
    public class StockAdjustmentController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/StockAdjustment
        public IEnumerable<StockAdjustment> Get()
        {
            return db.StockAdjustments.Where(st => st.IsSync == false);
        }

        // POST: api/StockAdjustment
        public HttpResponseMessage Post(StockAdjustment adjust)
        {
            try
            {
                var r = db.StockAdjustments.Find(adjust.ID);

                if (r != null)
                {
                    db.Entry(adjust).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.StockAdjustments.Add(adjust);
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
