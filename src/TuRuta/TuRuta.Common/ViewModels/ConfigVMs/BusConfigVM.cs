using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.Common.ViewModels.ConfigVMs
{
    public class BusConfigVM
    {
        public Guid BusId { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
