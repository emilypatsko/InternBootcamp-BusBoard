using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace BusBoard
{
    public class TransportApi
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly string _credentials;
        public TransportApi(IConfiguration config)
        {
            _config = config;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(@"http://transportapi.com/v3/uk/");
            _credentials = $"app_id={_config["ApiSecrets:AppId"]}&app_key={_config["ApiSecrets:AppKey"]}";
        }

        public async Task<List<BusStopResponse>> GetNearestStops(string latitude, string longitude)
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

        public async Task<DeparturesResponse> GetDepartures(string stopCode)
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

