﻿using MoostBrand.Areas.WebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MoostBrand.Areas.WebService.Controllers
{
    public class StockAdjustmentDetailsController : ApiController
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        // GET: api/StockAdjustmentDetails
        public IEnumerable<StockAdjustmentDetail> Get()
        {
            return db.StockAdjustmentDetails.Where(st => st.IsSync == false);
        }

        // POST: api/StockAdjustmentDetails
        public HttpResponseMessage Post(StockAdjustmentDetail adjust)
        {
            try
            {
                adjust.IsSync = true;

                db.StockAdjustmentDetails.Add(adjust);
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
