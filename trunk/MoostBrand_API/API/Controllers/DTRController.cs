using API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace API.Controllers
{
    [System.Web.Http.RoutePrefix("api/DTR")]
    public class DTRController : ApiController
    {
        // GET: DTR
        /// <summary>
        ///insert DTR
        /// </summary>
        /// <param name="token">apikey(string),secret(string),empID(string),scanDate(datetime),logType(bool)</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("InserDTR")]
        public HttpResponseMessage InserDTR(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string apiKey = string.Empty;
            string secret = string.Empty;
            string empID = string.Empty;
            DateTime scanDate = DateTime.Now;
            bool? logType= null; 

            //get payload key's and value's
            foreach (KeyValuePair<string, object> _pair in tokenS.Payload)
            {
                string key = _pair.Key;
                string value = _pair.Value.ToString();

                if (key == "apikey")
                    apiKey = value;

                if (key == "secret")
                    secret = value;

                if (key == "empID")
                    empID = value;

                if (key == "scanDate")
                    scanDate = Convert.ToDateTime(value);

                if (key == "logType")
                    logType = Convert.ToBoolean(value);

            }

            APICredential _credential = new APICredential();

            //validate credential  
            if (_credential.Validate(apiKey, secret))
            {

                Moostbrand _entities = new Moostbrand();

                try
                {
                    //insert to dtrlogs
                    dtrLog _entity = new dtrLog();

                    _entity.empID = empID;
                    _entity.scanDate = scanDate;
                    _entity.logType = logType;
 
                    _entities.dtrLogs.Add(_entity);
                    _entities.SaveChanges();
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
}