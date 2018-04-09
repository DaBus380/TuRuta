using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using TuRuta.Common.ViewModels;

namespace TuRuta.Client.Stops
{
    class StopsClient
    {
		public StopsClient(string baseAddress = null)
		{
			var address = baseAddress ?? "http://localhost:56340/";
			HttpClient.BaseAddress = new Uri(address);
		}

		private HttpClient HttpClient { get; } = new HttpClient();
	}
}
