using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TuRuta.Common.Models;

namespace TuRuta.Common.ViewModels
{
    [Serializable]
    [DataContract]
    public class StopVM
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Point Location { get; set; }
    }
}