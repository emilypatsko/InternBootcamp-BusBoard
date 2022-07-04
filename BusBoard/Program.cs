using Microsoft.Extensions.Configuration;

namespace BusBoard
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\EmiPat\source\repos\Bootcamp\BusBoard\BusBoard\appsettings.Development.json");
            var config = builder.Build();
            var api = new TransportApi(config);

            Console.WriteLine("Welcome to BusBoard!");
            Console.Write("Please enter a bus stop code: ");
            var stopCode = Console.ReadLine();
            var departures = await api.GetDepartures(stopCode);
            PrintDepartures(departures);
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
