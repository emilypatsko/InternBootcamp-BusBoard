namespace BusBoard
{
    class Program
    {
        private static readonly TransportApi TransportApi = new TransportApi();
        private static readonly PostcodeApi PostcodeApi = new PostcodeApi();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to BusBoard!");
            Console.Write("Please enter a postcode: ");
            var postcode = Console.ReadLine();
            await GetAndPrintBuses(postcode);
        }

        private static async Task GetAndPrintBuses(string? postcode)
        {
            if (String.IsNullOrEmpty(postcode))
            {
                return;
            }

            var postcodeResponse = await PostcodeApi.GetLatitudeAndLongitude(postcode);
            var stops = await TransportApi.Get2NearestStops(postcodeResponse);
            var stopsAndDepartures = await TransportApi.GetDeparturesForStops(stops);
            PrintDeparturesForStops(stopsAndDepartures);
        }

        private static void PrintDeparturesForStops(List<DeparturesResponse> stopsAndDepartures)
        {
            stopsAndDepartures.ForEach(PrintDeparturesForStop);
        }

        private static void PrintDeparturesForStop(DeparturesResponse stopAndDepartures)
        {
            // Flatten dictionary of buses (indexed by bus line) to list of buses
            var buses = stopAndDepartures.Departures.Values.ToList().SelectMany(x => x)
                .OrderBy(bus => bus.ExpectedDepartureTime)
                .Take(5)
                .ToList();

            // Figure out how wide the table needs to be (some bus directions are quite long...)
            var dirColWidth = buses.Max(b => b.Direction.Length) + 5;
            var dividerLength = 45 + dirColWidth;

            Console.WriteLine();
            Console.WriteLine($"Bus departures from {stopAndDepartures.StopName}");
            Console.WriteLine(new String('=', dividerLength));
            Console.WriteLine("{0,-15}{1}{2,-15}{3,-15}", "Line Number", "Direction".PadRight(dirColWidth), "Aimed", "Expected");
            Console.WriteLine(new String('=', dividerLength));
            buses.ForEach(bus => Console.WriteLine("{0,-15}{1}{2,-15}{3,-15}",
                bus.LineNumber,
                bus.Direction.PadRight(dirColWidth),
                bus.AimedDepartureTime,
                bus.ExpectedDepartureTime));
        }
    }
}
