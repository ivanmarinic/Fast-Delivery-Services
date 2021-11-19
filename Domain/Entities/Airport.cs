using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Airport
    {
        public string IATA { get; set; }
        public string AirportName { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }

        public static implicit operator Airport(int v)
        {
            throw new NotImplementedException();
        }
    }
}
