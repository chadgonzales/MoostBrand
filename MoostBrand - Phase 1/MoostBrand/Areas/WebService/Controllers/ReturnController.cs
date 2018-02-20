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
    public class ReturnController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/Return
        public IEnumerable<Return> Get()
        {
            return db.Returns.Where(st => st.IsSync == false);
        }

        // POST: api/Return
        public HttpResponseMessage Post(Return retrn)
        {
            try
            {
                retrn.IsSync = true;

                db.Returns.Add(retrn);
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
