using System;
using System.Threading;

namespace MediatorPatterm
{
    class Program
    {
        static void Main(string[] args)
        {
            var vehicle1 = new Vehicle("Reg1");
            var vehicle2 = new Vehicle("Reg2");
            var vehicle3 = new Vehicle("Reg3");

            var controlCenter = new ControlCenter();

            for (int i = 0; i <= 10; i++)
            {
                vehicle1.Move();
                vehicle2.Move();
                vehicle3.Move();
                Console.WriteLine("==========================================================================");
                Thread.Sleep(5000);
            }

        }
    }

    public class Mediator
    {

        //make it singleton
        private static readonly Mediator _instance = new Mediator();

        //hide constructor
        private Mediator() 
        { 
        }

        //provide a way to get instance
        public static Mediator Instance => _instance;

        //Instance Functionality
        public event EventHandler<LocationChangedEventArgs> LocationChanged;

        public void OnLocationChanged(object sender, LocationChangedEventArgs locationInfo)
        {
            LocationChanged?.Invoke(sender, locationInfo);
        }


    }

    public class LocationChangedEventArgs : EventArgs
    {
        public string RegNo { get; set; }
        public Location Location { get; set; }
    }
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class ControlCenter
    {
        public ControlCenter()
        {
            Mediator.Instance.LocationChanged += (s, e) => {
                Console.WriteLine($"Vehicle {e.RegNo} Moved! Location -> [{e.Location.Longitude}, {e.Location.Latitude}]");
            };
        }
    }
    public class Vehicle
    {
        public string RegNo { get; set; }
        public Location CurrentLocation { get; set; }
        private Random _random = new Random();

        public Vehicle(string regNo)
        {
            this.RegNo = regNo;
            this.CurrentLocation = new Location();
        }

        public void Move()
        {
            this.CurrentLocation.Longitude = _random.Next(0, 100);
            this.CurrentLocation.Latitude = _random.Next(0, 100);

            //broadcasting location

            Mediator.Instance.OnLocationChanged(this,
                new LocationChangedEventArgs { RegNo = this.RegNo, Location = this.CurrentLocation });
        }
    }
}
