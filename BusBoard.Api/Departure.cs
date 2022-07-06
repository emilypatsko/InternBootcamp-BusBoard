using Newtonsoft.Json;

namespace BusBoard
{
    public class Departure
    {
        [JsonProperty("line")]
        public string LineNumber { get; set; }
        [JsonProperty("direction")]
        public string Direction { get; set; }
        [JsonProperty("aimed_departure_time")]
        public string AimedDepartureTime { get; set; }
        [JsonProperty("expected_departure_time")]
        public string ExpectedDepartureTime { get; set; }
    }
}
