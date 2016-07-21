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
    public class StockTransferDetailsController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/StockTransferDetails
        public IEnumerable<StockTransferDetail> Get()
        {
            return db.StockTransferDetails.Where(st => st.IsSync == false);
        }
        
        // POST: api/StockTransferDetails
        public HttpResponseMessage Post(StockTransferDetail stocktransfer)
        {
            try
            {
                stocktransfer.IsSync = true;

                db.StockTransferDetails.Add(stocktransfer);
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
