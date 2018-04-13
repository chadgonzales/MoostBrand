using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class APICredential
    {
        /// <summary>
        /// validate api credential
        /// </summary>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public bool Validate(string key, string secret)
        {
            if ((key == ConfigurationManager.AppSettings["apiKey"].ToString()) && (secret == ConfigurationManager.AppSettings["apiSecret"].ToString()))
                return true;

            return false;
        }
    }
}