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
    public class StockAllocationController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/StockAllocation
        public IEnumerable<StockAllocation> Get()
        {
            return db.StockAllocations.Where(st => st.IsSync == false);
        }

        // POST: api/StockAllocation
        public HttpResponseMessage Post(StockAllocation allocation)
        {
            try
            {
                var r = db.StockAllocations.Find(allocation.ID);

                if (r != null)
                {
                    db.Entry(allocation).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.StockAllocations.Add(allocation);
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
