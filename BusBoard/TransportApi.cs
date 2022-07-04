using Microsoft.Extensions.Configuration;

namespace BusBoard
{
    public class TransportApi
    {
        private readonly IConfiguration _config;

        private readonly HttpClient _client;
        public TransportApi(IConfiguration config)
        {
            _config = config;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(@"http://transportapi.com/v3/uk/bus/");
        }

        public async Task<DeparturesResponse> GetDepartures(string stopCode)
        {
            var departures = new DeparturesResponse();

            var parameters = $"stop/{stopCode}/live.json?app_id={_config["ApiSecrets:AppId"]}&app_key={_config["ApiSecrets:AppKey"]}";
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

