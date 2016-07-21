using Synchronizer.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Synchronizer.Repository
{
    class Returns : API
    {
        private async Task<List<Return>> GetUnsyc(string URL)
        {
            try
            {
                this.URL = URL;

                string content = string.Empty;

                var response = await this.Get("api/Return");

                content = await response.Content.ReadAsStringAsync();

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                return json_serializer.Deserialize<List<Return>>(content);
            }
            catch
            {
            }

            return null;
        }

        public async Task<string> Post(string URL, Return _entity)
        {
            this.URL = URL;

            var response = await this.Post(_entity, "api/Return");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<Return>> GetUnsyncLocal()
        {
            string URL = ConfigurationManager.AppSettings["urlLocal"].ToString();

            return await GetUnsyc(URL);
        }

        public async Task<List<Return>> GetUnsyncLive()
        {
            string URL = ConfigurationManager.AppSettings["urlLive"].ToString();

            return await GetUnsyc(URL);
        }

        public async Task<string> PostLocal(Return _entity)
        {
            string URL = ConfigurationManager.AppSettings["urlLocal"].ToString();

            return await Post(URL, _entity);
        }

        public async Task<string> PostLive(Return _entity)
        {
            string URL = ConfigurationManager.AppSettings["urlLive"].ToString();

            return await Post(URL, _entity);
        }
    }
}
