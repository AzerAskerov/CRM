//using System;
//using System.Collections.Generic;
//using System.Linq;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authentication.Cookies;


//namespace CRM.Server.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        private readonly ILogger<WeatherForecastController> logger;

//        public WeatherForecastController(ILogger<WeatherForecastController> logger)
//        {
//            this.logger = logger;
//        }


//        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
//        [HttpGet]
//        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public IEnumerable<WeatherForecast> Get()
//        {
//            var rng = new Random();
//            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//            {
//                Date = DateTime.Now.AddDays(index),
//                TemperatureC = rng.Next(-20, 55),
//                Summary = Summaries[rng.Next(Summaries.Length)]
//            })
//            .ToArray();
//        }


//    }
//}
