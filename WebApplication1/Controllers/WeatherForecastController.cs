using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


public class weather
{
    public string description { get; set; }
}

public class main
{
    public float temp { get; set; }
    public float feels_like { get; set; }
}

public class wind
{
    public float speed { get; set; }
}

public class WeatherResponse
{
    [JsonPropertyName("weather")]
    public List<weather> Weather { get; set; }
    [JsonPropertyName("main")]
    public main main { get; set; }
    [JsonPropertyName("wind")]
    public wind wind { get; set; }
    [JsonPropertyName("name")]
    public string name { get; set; }

}


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            string apikey = "d7e87144f2058845782a30865da07674";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apikey}&units=metric";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonresult = await response.Content.ReadAsStringAsync();
                    var weather = JsonSerializer.Deserialize<WeatherResponse>(jsonresult);
                    return Ok(new
                    {
                        City = weather.name,
                        Temperature = weather.main.temp,
                        FeelsLike = weather.main.feels_like,
                        WindSpeed = weather.wind.speed,
                        Description = weather.Weather[0].description
                    });
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error calling OpenWeatherMap API");
                }
            }
        }
       
    }
}
