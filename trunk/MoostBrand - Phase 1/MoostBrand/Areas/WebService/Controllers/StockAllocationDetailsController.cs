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
    public class StockAllocationDetailsController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/StockAllocationDetails
        public IEnumerable<StockAllocationDetail> Get()
        {
            return db.StockAllocationDetails.Where(st => st.IsSync == false);
        }

        // POST: api/StockAllocationDetails
        public HttpResponseMessage Post(StockAllocationDetail allocation)
        {
            try
            {
                allocation.IsSync = true;

                db.StockAllocationDetails.Add(allocation);
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
