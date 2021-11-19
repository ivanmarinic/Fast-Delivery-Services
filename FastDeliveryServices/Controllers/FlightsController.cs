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
    public class FlightsController : ControllerBase
    {
        private static IConfiguration configuration;
        private static ConfigurationBuilder builder;

        public FlightsController()
        {
            builder = (ConfigurationBuilder)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            configuration = builder.Build();
        }

        [HttpGet]
        public List<Airport> Get()
        {
            ConnectDb db = new ConnectDb(configuration);

            return db.GetAirports();
        }


        [HttpPost]
        public async Task<ActionResult<Flight>> Post(string IATAFrom, string IATATo)
        {
            try
            {
                ConnectDb db = new ConnectDb(configuration);

                db.GetFlights(IATAFrom, IATATo);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }

            return this.StatusCode(StatusCodes.Status200OK, "Success");
        }

        [HttpDelete]
        public async Task<ActionResult<Flight>> Delete(string IATAFrom, string IATATo)
        {
            try
            {
                ConnectDb db = new ConnectDb(configuration);

                db.DeleteFlights(IATAFrom, IATATo);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }

            return this.StatusCode(StatusCodes.Status200OK, "Success");
        }
    }
}
