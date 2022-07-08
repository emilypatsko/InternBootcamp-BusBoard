using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace BusBoard
{
    public class TransportApi
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly string _credentials;
        public TransportApi()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\EmiPat\source\repos\Bootcamp\BusBoard\BusBoard.Api\appsettings.Development.json");
            _config = builder.Build();
            _client = new HttpClient();
            _client.BaseAddress = new Uri(@"http://transportapi.com/v3/uk/");
            _credentials = $"app_id={_config["ApiSecrets:AppId"]}&app_key={_config["ApiSecrets:AppKey"]}";
        }

        public async Task<List<BusStopResponse>> Get2NearestStops(PostcodeResponse postcodeResponse)
        {
            return await Get2NearestStops(postcodeResponse.Latitude, postcodeResponse.Longitude);
        }

        private async Task<List<BusStopResponse>> Get2NearestStops(string latitude, string longitude)
        {
            var busStops = new List<BusStopResponse>();
            var parameters = $"places.json?type=bus_stop&lat={latitude}&lon={longitude}&{_credentials}";
            HttpResponseMessage response = await _client.GetAsync(parameters);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonString);
                List<JToken> results = jsonObject["member"].Children().ToList();
                results.Take(2).ToList().ForEach(stop => busStops.Add(stop.ToObject<BusStopResponse>()));
            }

            return busStops;
        }

        public async Task<List<DeparturesResponse>> GetDeparturesForStops(List<BusStopResponse> busStops)
        {
            var stopsAndDepartures = new List<DeparturesResponse>();
            foreach (var stop in busStops)
            {
                stopsAndDepartures.Add(await GetDepartures(stop.StopCode));
            }

            return stopsAndDepartures;
        }

        private async Task<DeparturesResponse> GetDepartures(string stopCode)
        {
            var departures = new DeparturesResponse();

            var parameters = $"bus/stop/{stopCode}/live.json?{_credentials}";
            HttpResponseMessage response = await _client.GetAsync(parameters);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                departures = Newtonsoft.Json.JsonConvert.DeserializeObject<DeparturesResponse>(jsonString);
            }

            return departures;
        }
    }
}

