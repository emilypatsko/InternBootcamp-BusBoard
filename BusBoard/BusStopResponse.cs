using Newtonsoft.Json;

namespace BusBoard;

public class BusStopResponse
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("atcocode")]
    public string StopCode { get; set; }
}
