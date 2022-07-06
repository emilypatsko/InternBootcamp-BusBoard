using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace BusBoard
{
    public class PostcodeApi
    {
        private readonly HttpClient _client;
        private readonly Regex _postcodeExpr = new Regex(@"^([A-Z]{1,2}\d[A-Z\d]? ?\d[A-Z]{2}|GIR ?0A{2})$", RegexOptions.IgnoreCase);

        public PostcodeApi()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(@"http://api.postcodes.io/postcodes/");
        }

        private bool IsPostcodeValid(string postcode)
        {
            return _postcodeExpr.IsMatch(postcode);
        }

        public async Task<PostcodeResponse> GetLatitudeAndLongitude(string postcode)
        {
            var postcodeResponse = new PostcodeResponse();

            if (IsPostcodeValid(postcode))
            {
                HttpResponseMessage response = await _client.GetAsync(postcode);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonString);
                    postcodeResponse = jsonObject["result"].ToObject<PostcodeResponse>();
                }
            }

            return postcodeResponse;
        }
    }
}

