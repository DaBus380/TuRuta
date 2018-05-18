using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.Common.Models
{
    public class Stop
    {
        public Point Location { get; set; }
		public string Name { get; set; }
        public Guid Id { get; set; }
	}
}
