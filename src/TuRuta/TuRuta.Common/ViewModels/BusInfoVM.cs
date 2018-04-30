using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.Common.ViewModels
{
    [Serializable]
    public class BusInfoVM
    {
        public string PreviewsStop { get; set; }
        public string CurrentStop { get; set; }
        public string NextStop { get; set; }
    }
}
