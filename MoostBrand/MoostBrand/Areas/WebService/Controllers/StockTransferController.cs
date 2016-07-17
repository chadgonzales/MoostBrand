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
    public class StockTransferController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/StockTransfer
        public IEnumerable<StockTransfer> Get()
        {
            return db.StockTransfers.Where(st => st.IsSync == false);
        }
        
        // POST: api/StockTransfer
        public HttpResponseMessage Post(StockTransfer stocktransfer)
        {
            try
            {
                var st = db.StockTransfers.Find(stocktransfer.ID);

                if(st != null)
                {
                    db.Entry(stocktransfer).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.StockTransfers.Add(stocktransfer);
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
