namespace BusBoard
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var transportApi = new TransportApi();
            var postcodeApi = new PostcodeApi();

            Console.WriteLine("Welcome to BusBoard!");
            Console.Write("Please enter a postcode: ");
            var postcode = Console.ReadLine();
            var postcodeResponse = await postcodeApi.GetLatitudeAndLongitude(postcode);
            var stops = await transportApi.GetNearestStops(postcodeResponse.Latitude, postcodeResponse.Longitude);
            foreach (var stop in stops)
            {
                var departures = await transportApi.GetDepartures(stop.StopCode);
                PrintDepartures(departures);
            }
        }

        private static void PrintDepartures(DeparturesResponse departures)
        {
            Console.WriteLine();
            Console.WriteLine($"Bus departures from {departures.StopName}");
            Console.WriteLine("======================================================================");
            var buses = departures.Departures.Values.ToList().SelectMany(x => x)
                .OrderBy(bus => bus.ExpectedDepartureTime)
                .Take(5)
                .ToList();

            Console.WriteLine("{0,-15}{1,-25}{2,-15}{3,-15}", "Line Number", "Direction", "Aimed", "Expected");
            Console.WriteLine("======================================================================");
            buses.ForEach(bus => Console.WriteLine("{0,-15}{1,-25}{2,-15}{3,-15}",
                bus.LineNumber,
                bus.Direction,
                bus.AimedDepartureTime,
                bus.ExpectedDepartureTime));
        }
    }
}
