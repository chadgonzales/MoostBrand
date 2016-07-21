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
    public class ReceivingController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/Receiving
        public IEnumerable<Receiving> Get()
        {
            return db.Receivings.Where(st => st.IsSync == false);
        }

        // POST: api/Receiving
        public HttpResponseMessage Post(Receiving receiving)
        {
            try
            {
                receiving.IsSync = true;

                db.Receivings.Add(receiving);
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
