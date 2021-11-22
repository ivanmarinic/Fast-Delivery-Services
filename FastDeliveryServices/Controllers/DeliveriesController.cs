using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using Persistence;

namespace FastDeliveryServices.Controllers
{
    [Route("api/fad/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private static IConfiguration configuration;
        private static ConfigurationBuilder builder;

        public DeliveriesController()
        {
            builder = (ConfigurationBuilder)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            configuration = builder.Build();
        }

        
        [HttpGet]
        public async Task<ActionResult> Get(string IATAFrom, string IATATo, DateTime date)
        {
            
            //looking if delivery can be made
            //if delivery can be made, it calculates the distance of the path
            try
            {
                ConnectDb db = new ConnectDb(configuration);
                
                var airports = db.GetAirports();

                var flights = db.GetListOfFlights();

                Graph g = new Graph(airports.Count);

                Dictionary<string, int> airportValues = new Dictionary<string, int>();

                for (int i = 0; i < airports.Count; i++)
                {
                    airportValues[airports[i].IATA] = i;
                }

                for (int i = 0; i < flights.Count; i++)
                {
                    g.addEdge(airportValues[flights[i].IATAFrom], airportValues[flights[i].IATATo]);
                    g.addEdge(airportValues[flights[i].IATATo], airportValues[flights[i].IATAFrom]);
                }


                int s = airportValues[IATAFrom];

                int d = airportValues[IATATo];

                var list = g.printAllPaths(s, d);

                List<string> IATAPath = new List<string>();

                List<int> numbers = new List<int>();

                foreach(string path in list)
                {
                    numbers = path.Split(' ').Select(Int32.Parse).ToList();
                }

                

                Decimal PathDistanceTotal = 0;
                Decimal TotalPrice = 0;

                var PathKeys = new List<string>();
                

                for (int i = 0; i < numbers.Count; i++)
                {
                    PathKeys.Add(airportValues.FirstOrDefault(x => x.Value == numbers[i]).Key);
                }

                for (int i = 0; i < numbers.Count - 1; i++)
                {
                    PathDistanceTotal += db.GetPathDistanceInNauticalMiles(PathKeys[i], PathKeys[i + 1]);
                }

                var DayInTheWeek = date.DayOfWeek;

                TotalPrice += db.GetCostOfDelivery(DayInTheWeek.ToString(), PathDistanceTotal);

                if(TotalPrice > 0)
                    return this.StatusCode(StatusCodes.Status200OK, new JsonResult(new { Status = "AVAILABLE", Message = "Thank you for using FAD services", Departure = IATAFrom, Arrival = IATATo, TotalPrice = TotalPrice }));
                else
                    return this.StatusCode(StatusCodes.Status200OK, new JsonResult(new { Status = "UNAVAILABLE", Message = "We are unable to fulfill your request :(", Departure = IATAFrom, Arrival = IATATo, TotalPrice = 0 }));
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, new JsonResult(new { Status = "ERROR", Message = ex }));
            }

            //return this.StatusCode(StatusCodes.Status200OK, new JsonResult(new { message = "Success" }));
        }
    }
}
