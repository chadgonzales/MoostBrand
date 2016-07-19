using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MoostBrand.Models
{
    public class Repo : API
    {
        public async Task<string> PostLive(R _entity)
        {
            this.URL = ConfigurationManager.AppSettings["urlLive"].ToString();

            var response = await this.Post(_entity, "api/Requisition");

            return await response.Content.ReadAsStringAsync();
        }
    }
}