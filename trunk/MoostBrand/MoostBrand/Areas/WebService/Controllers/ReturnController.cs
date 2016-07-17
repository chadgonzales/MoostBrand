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
                var r = db.Returns.Find(retrn.ID);

                if (r != null)
                {
                    db.Entry(retrn).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.Returns.Add(retrn);
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
