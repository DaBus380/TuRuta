using System;
using System.Runtime.Serialization;

namespace TuRuta.Common.ViewModels
{
    [Serializable]
    [DataContract]
    public class BusVM
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string LicensePlate { get; set; }
        [DataMember]
        public int Status { get; set; }
    }
}