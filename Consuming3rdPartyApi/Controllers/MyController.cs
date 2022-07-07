using Consuming3rdPartyApi.models;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace Consuming3rdPartyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumeApiController : ControllerBase
    {

        //private string url = "https://api-omnichannel-dev.azure-api.net/v1/datalookup/telcos";

        private string url = "http://api.weatherapi.com/v1/current.json?key=f8a90ad946c147b49a7102324222906&q=London&aqi=no";
        //private HttpClientUtil _util;
        HttpClient client = new HttpClient();
        public ConsumeApiController()
        {

        }

        [HttpGet("newEnd")]
        public async Task<IActionResult> GetWeatherAsync(string city)
        {
            using (var cl = new HttpClient())
            {
                //cl.BaseAddress = new Uri(url);
                var url = "http://api.weatherapi.com/v1/current.json?key=f8a90ad946c147b49a7102324222906&q="+ city+"&aqi=no";
                var response = await cl.GetAsync(url);
                if(response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseObjects = System.Text.Json.JsonSerializer.Deserialize<WeatherResponse>(responseString);
                    return Ok(responseObjects);
                }
                return BadRequest();
            }

        }
        [HttpGet("anotherEnd")]
        public IActionResult GetWeatherRestSharp(string city)
        {

            var client = new RestClient("http://api.weatherapi.com/v1/current.json?key=f8a90ad946c147b49a7102324222906&q="+city+"&aqi=no");
            
            var request = new RestRequest();
            RestResponse response = client.Get(request);

            var responseObject = JsonConvert.DeserializeObject<WeatherResponse>(response.Content);
            return Ok(responseObject.Location);

        }
        [HttpPost("postdata")]
        public IActionResult PostData(SuperHero hero)
        {
            using (var cl = new HttpClient())
            {
                var endpoint = new Uri("https://localhost:7095/api/SuperHero");
                var pstjson = JsonConvert.SerializeObject(hero);
                var payLoad = new StringContent(pstjson, Encoding.UTF8, "application/json");

                var result = cl.PostAsync(endpoint, payLoad).Result.Content.ReadAsStringAsync().Result;

                return Ok("User created successfully"+ result);
            }
        }
        [HttpPost("details")]
       /* public IActionResult GetDetails([FromBody])
        {
            return Ok();
        }*/

        [HttpGet]
        public async Task<IActionResult> FetchData()
        {
            List<Telco>? cs = new List<Telco>();
            HttpResponseMessage res = await client.GetAsync(this.url);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                cs = JsonConvert.DeserializeObject<List<Telco>>(result);

                return Ok(cs!);
            }
   
                return BadRequest();
            
        }

     /*   [HttpGet("getTelcos")]
        public IActionResult GetTelcos()
        {
            var telcos = _util.GetJSON<List<Telco>>(url);
            return Ok(telcos);
        }*/
    }
    public class SuperHero
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;

    }
}
