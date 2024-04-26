using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using test.Constants;

namespace test.Controllers.Questions3Controller
{
    [Route("api/questions/[controller]")]
    [ApiController]
    public class Question3 : Controller
    {
        public class Q3Request
        {
           public DateTime Date { get; set; }
        }

        public class CovidCase
        {
            public string New { get; set; }
            public int Active { get; set; }
            public int Critical { get; set; }
            public int Recovered { get; set; }
            [JsonProperty("1M_pop")]
            public string CasesPerMillionPopulation { get; set; }
            public int Total { get; set; }
        }

        public class CovidDeath
        {
            public string New { get; set; }
            [JsonProperty("1M_pop")]
            public string DeathsPerMillionPopulation { get; set; }
            public int Total { get; set; }
        }

        public class CovidTest
        {
            [JsonProperty("1M_pop")]
            public string DeathsPerMillionPopulation { get; set; }
            public int Total { get; set; }
        }

        public class CovidData
        {
            public string Continent { get; set; }
            public string Country { get; set; }
            public int Population { get; set; }
            public CovidCase Cases { get; set; }
            public CovidDeath Deaths { get; set; }
            public CovidTest Tests { get; set; }
            public string Day { get; set; }
            public string Time { get; set; }
        }

        public class CovidResponse
        {

            public string Get { get; set; }
            public Dictionary<string, string> Parameters { get; set; }
            public List<object> Errors { get; set; }
            public int Results { get; set; }
            public List<CovidData> Response { get; set; }
        }

        public class Result
        {
            public string url { get; set; }
            public string method { get; set; }
            public CovidResponse response { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<Result>> GetCovidReport([FromQuery] Q3Request request)
        {
            try
            {
                DateTime date = request.Date.ToUniversalTime();
                var client = new HttpClient();
                var apiRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://covid-193.p.rapidapi.com/history?country=usa&day={request.Date.ToString("yyyy-MM-dd", new CultureInfo("en-US"))}")
                };

                apiRequest.Headers.Add("X-RapidAPI-Key", TestConstants.CovidApi.ApiKey);
                apiRequest.Headers.Add("X-RapidAPI-Host", TestConstants.CovidApi.Host);

                using (var response = await client.SendAsync(apiRequest))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var covidResponse = JsonConvert.DeserializeObject<CovidResponse>(body);

                    Result result = new Result();
                    result.url = apiRequest.RequestUri.ToString();
                    result.method = apiRequest.Method.ToString();
                    result.response = covidResponse;
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
