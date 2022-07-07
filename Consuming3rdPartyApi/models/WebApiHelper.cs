using Newtonsoft.Json;

namespace Consuming3rdPartyApi.models
{
    public class WebApiHelper
    {
        string url = string.Empty;
        HttpClient client = new HttpClient();

        public WebApiHelper(string url)
        {
            this.url = url;
        }

        public async Task<List<Telco>> GetData()
        {
            List<Telco>? cs = new List<Telco>();
            HttpResponseMessage res = await client.GetAsync(this.url);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                cs = JsonConvert.DeserializeObject<List<Telco>>(result);
            }
            //HttpResponseMessage response = client.GetAsync(this.url).Result;
            //var data = JsonConvert.DeserializeObject<Telco[]>(response.Content.ReadAsStringAsync().Result);

            return cs!;
        }

       
    }
}
