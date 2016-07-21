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
    class StockTransferDetails : API
    {
        private async Task<List<StockTransferDetail>> GetUnsyc(string URL)
        {
            try
            {
                this.URL = URL;

                string content = string.Empty;

                var response = await this.Get("api/StockTransferDetails");

                content = await response.Content.ReadAsStringAsync();

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                return json_serializer.Deserialize<List<StockTransferDetail>>(content);
            }
            catch
            {
            }

            return null;
        }

        public async Task<string> Post(string URL, StockTransferDetail _entity)
        {
            this.URL = URL;

            var response = await this.Post(_entity, "api/StockTransferDetails");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<StockTransferDetail>> GetUnsyncLocal()
        {
            string URL = ConfigurationManager.AppSettings["urlLocal"].ToString();

            return await GetUnsyc(URL);
        }

        public async Task<List<StockTransferDetail>> GetUnsyncLive()
        {
            string URL = ConfigurationManager.AppSettings["urlLive"].ToString();

            return await GetUnsyc(URL);
        }

        public async Task<string> PostLocal(StockTransferDetail _entity)
        {
            string URL = ConfigurationManager.AppSettings["urlLocal"].ToString();

            return await Post(URL, _entity);
        }

        public async Task<string> PostLive(StockTransferDetail _entity)
        {
            string URL = ConfigurationManager.AppSettings["urlLive"].ToString();

            return await Post(URL, _entity);
        }
    }
}
