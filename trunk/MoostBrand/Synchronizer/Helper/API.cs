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
        public async Task<HttpResponseMessage> Get(string urlParam)
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

        public async Task<HttpResponseMessage> Post(object _entity, string _urlParam)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(URL);

                    HttpResponseMessage response = await client.PostAsJsonAsync(_urlParam, _entity);

                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }
                catch(Exception ex)
                {
                    string s = ex.Message;
                }

                return null;
            }
        }

        public async Task<HttpResponseMessage> Put(object _entity, string _urlParam)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(URL);

                    HttpResponseMessage response = await client.PutAsJsonAsync(_urlParam, _entity);

                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }

                return null;
            }
        }
    }
}
