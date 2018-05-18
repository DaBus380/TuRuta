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
        public List<BusVM> Buses { get; set; } = new List<BusVM>();
        [DataMember]
        public List<StopVM> Stops { get; set; } = new List<StopVM>();
        [DataMember]
        public List<IncidentVM> Incidents { get; set; } = new List<IncidentVM>();
    }
}