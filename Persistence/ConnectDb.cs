using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public class ConnectDb
    {
        private string connectionString;
        public ConnectDb(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Airport> GetAirports()
        {
            var airports = new List<Airport>();
            try
            {
                using var con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GetAirports", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();

                while (read.Read())
                {
                    airports.Add(new Airport
                    {
                        IATA = Convert.ToString(read[0]),
                        AirportName = Convert.ToString(read[1]),
                        Latitude = Convert.ToDecimal(read[2]),
                        Longitude = Convert.ToDecimal(read[3])
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
            return airports;
        }

        public void GetFlights(string IATAFrom, string IATATo)
        {
            try
            {
                using var con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GetFlights", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@IATAFrom", System.Data.SqlDbType.VarChar).Value = IATAFrom;
                cmd.Parameters.Add("@IATATo", System.Data.SqlDbType.VarChar).Value = IATATo;
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteFlights(string IATAFrom, string IATATo)
        {
            try
            {
                using var con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("DeleteFlights", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@IATAFrom", System.Data.SqlDbType.VarChar).Value = IATAFrom;
                cmd.Parameters.Add("@IATATo", System.Data.SqlDbType.VarChar).Value = IATATo;
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Flight> GetListOfFlights()
        {
            var flights = new List<Flight>();
            try
            {
                using var con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GetListOfFlights", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();

                while (read.Read())
                {
                    flights.Add(new Flight
                    {
                        IATAFrom = Convert.ToString(read[0]),
                        IATATo = Convert.ToString(read[1])
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
            return flights;
        }

        public Decimal GetPathDistanceInNauticalMiles(string IATAFirstAirport, string IATASecondAirport)
        {
            Decimal PathDistanceInNauticalMiles = 0;
            try
            {
                using var con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GetPathDistanceInNauticalMiles", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@IATAFirstAirport", System.Data.SqlDbType.VarChar).Value = IATAFirstAirport;
                cmd.Parameters.Add("@IATASecondAirport", System.Data.SqlDbType.VarChar).Value = IATASecondAirport;
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();

                while (read.Read())
                {
                    PathDistanceInNauticalMiles = Convert.ToDecimal(read[0]);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return PathDistanceInNauticalMiles;
        }

        public Decimal GetCostOfDelivery(string dayOfTheWeek, Decimal PathDistanceInNauticalMiles)
        {
            Decimal priceOfDelivery = 0;
            try
            {
                using var con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("GetCostOfDelivery", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@dayOfDelivery", System.Data.SqlDbType.VarChar).Value = dayOfTheWeek;
                cmd.Parameters.Add("@totalPathInMiles", System.Data.SqlDbType.Decimal).Value = PathDistanceInNauticalMiles;
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();

                while (read.Read())
                {
                    priceOfDelivery = Convert.ToDecimal(read[0]);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return priceOfDelivery;
        }



        


    }
}
