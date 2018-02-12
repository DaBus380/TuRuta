using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Windows.Devices.Gpio;

using TuRuta.Device.Configuration;
using TuRuta.Common.Device;
using TuRuta.Common.Enums;
using TuRuta.Common.ViewModels;

namespace TuRuta.Device
{
    public sealed class StartupTask : IBackgroundTask
    {
        ConfigurationClient configurationClient = new ConfigurationClient();
        private QueueClient queue;
        private Guid BusId;

        private BackgroundTaskDeferral taskDeferral;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskDeferral = taskInstance.GetDeferral();

            RunAsync().Wait();
        }

        private async Task RunAsync()
        {
            var blinker = Blink();
            var config = await configurationClient.GetConfig();

            BusId = config.BusId;

            queue = new QueueClient(config.ServiceBusConnectionString, config.QueueName);

            if(await GetLocationAccess())
            {
                var geolocator = GeolocatorBuilder();

                geolocator.PositionChanged += Geolocator_PositionChanged;
            }

            await blinker;
        }

        private async Task Blink()
        {
            var controller = await GpioController.GetDefaultAsync();
            var pin = controller.OpenPin(21);
            pin.SetDriveMode(GpioPinDriveMode.Output);
            while (true)
            {
                pin.Write(GpioPinValue.High);
                await Task.Delay(TimeSpan.FromSeconds(3));
                pin.Write(GpioPinValue.Low);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        private async Task<bool> GetLocationAccess()
        {
            var access = await Geolocator.RequestAccessAsync();
            return access == GeolocationAccessStatus.Allowed;
        }

        private Geolocator GeolocatorBuilder()
            => new Geolocator()
            {
                DesiredAccuracy = PositionAccuracy.Default,
                ReportInterval = Convert.ToUInt32(TimeSpan.FromMinutes(5).TotalMilliseconds),
                MovementThreshold = 5
            };

        private async void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var geoPoint = args.Position.Coordinate.Point.Position;
            var positionUpdate = new PositionUpdate()
            {
                Latitude = geoPoint.Latitude,
                Longitude = geoPoint.Longitude,
                Status = BusId != Guid.Empty ? BusStatus.Available : BusStatus.NotConfigured
            };

            var message = MessageBuilder(positionUpdate);

            await queue.SendAsync(message);
        }

        private Message MessageBuilder(PositionUpdate obj)
            => new Message(Serialize(obj))
            {
                To = BusId.ToString()
            };

        private byte[] Serialize(PositionUpdate obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF32.GetBytes(json);
        }
    }
}
