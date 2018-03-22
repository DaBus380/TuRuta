using System;
using System.Runtime.Serialization;

namespace TuRuta.Common.ViewModels
{
    [Serializable]
    [DataContract]
    public class IncidentVM
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Issue { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
