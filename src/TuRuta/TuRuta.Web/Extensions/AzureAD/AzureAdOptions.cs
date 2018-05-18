using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuRuta.Web.Extensions.AzureAD
{
    public class AzureAdOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string TenantId { get; set; }
    }
}
