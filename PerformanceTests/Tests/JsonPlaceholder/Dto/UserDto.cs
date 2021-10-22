using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests.Tests.JsonPlaceholder.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? UserName {  get; set; }

        public string? Email { get; set; }

        public AddressInfo? Address { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        public CompanyInfo? Company { get; set; }

        public class AddressInfo
        {
            public string? Street { get; set; }

            public string? Suite { get; set; }

            public string? City { get; set; }

            public string? Zipcode { get; set; }

            public GeoInfo? Geo { get; set; }

            public class GeoInfo
            {
                public string? Lat { get; set; }

                public string? Lng { get; set; }
            }
        }

        public class CompanyInfo
        { 
            public string? Name { get; set; }

            public string? CatchPhrase { get; set; }

            public string? Bs { get; set; }
        }
    }
}
