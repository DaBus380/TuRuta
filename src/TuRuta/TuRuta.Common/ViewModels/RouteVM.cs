using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TuRuta.Common.ViewModels
{
    [Serializable]
    [DataContract]
    public class RouteVM
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public IEnumerable<BusVM> Buses { get; set; }
        [DataMember]
        public IEnumerable<StopVM> Stops { get; set; }
        [DataMember]
        public IEnumerable<IncidentVM> Incidents { get; set; }
    }
}