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
    public class ReturnedItemsController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/ReturnedItems
        public IEnumerable<ReturnedItem> Get()
        {
            return db.ReturnedItems.Where(st => st.IsSync == false);
        }

        // POST: api/ReturnedItems
        public HttpResponseMessage Post(ReturnedItem retrn)
        {
            try
            {
                var r = db.ReturnedItems.Find(retrn.ID);

                if (r != null)
                {
                    db.Entry(retrn).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.ReturnedItems.Add(retrn);
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
