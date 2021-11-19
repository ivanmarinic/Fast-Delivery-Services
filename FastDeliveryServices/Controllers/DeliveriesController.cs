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
    [Route("api/[controller]")]
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




                // arbitrary source
                int s = airportValues[IATAFrom];

                // arbitrary destination
                int d = airportValues[IATATo];

                var list = g.printAllPaths(s, d);




                //db.GetDelivery(IATAFrom, IATATo, date);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }

            return this.StatusCode(StatusCodes.Status200OK, "Success");
        }

    }
}
