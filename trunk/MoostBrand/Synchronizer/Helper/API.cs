using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Helper
{
    class API
    {
        public string URL { get; set; }
        public async Task<HttpResponseMessage> MakeRequest(string urlParam)
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(URL + urlParam);

                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }
                catch
                {
                }
                return null;
            }
        }
    }
}
