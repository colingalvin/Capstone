using Capstone.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Services
{
    public class GoogleService
    {
        public async Task<TimeSpan> GetTravelTime(double originLat, double originLng, double destinationLat, double destinationLng)
        {
            TimeSpan travelTime = new TimeSpan(0, 0, 0);
            Uri directionsURL = new Uri($"https://maps.googleapis.com/maps/api/directions/json?origin={originLat},{originLng}&destination={destinationLat},{destinationLng}&key={ApiKeys.googleApiKey}");
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(directionsURL);

            if (response.IsSuccessStatusCode)
            {
                var task = response.Content.ReadAsStringAsync().Result;
                JObject directionsData = JsonConvert.DeserializeObject<JObject>(task);
                travelTime = new TimeSpan(0, 0, Convert.ToInt32(directionsData["routes"][0]["legs"][0]["duration"]["value"]));
                return travelTime;
            }

            return travelTime;
        }

        public async Task<PendingAppointment> GeocodeAddress(PendingAppointment pendingAppointment)
        {
            string parsedAddress = ParseAddress(pendingAppointment.StreetAddress, pendingAppointment.Zip);
            Uri directionsURL = new Uri($"https://maps.googleapis.com/maps/api/geocode/json?address={parsedAddress}&key={ApiKeys.googleApiKey}");
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(directionsURL);

            if (response.IsSuccessStatusCode)
            {
                var task = response.Content.ReadAsStringAsync().Result;
                JObject geocodeData = JsonConvert.DeserializeObject<JObject>(task);
                pendingAppointment.Latitude = Convert.ToDouble(geocodeData["results"][0]["geometry"]["location"]["lat"]);
                pendingAppointment.Longitude = Convert.ToDouble(geocodeData["results"][0]["geometry"]["location"]["lng"]);
                return pendingAppointment;
            }

            return pendingAppointment;
        }

        private string ParseAddress(string streetAddress, int zipCode)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < streetAddress.Length; i++)
            {
                if (streetAddress[i] == ' ')
                {
                    stringBuilder.Append("+");
                }
                else
                {
                    stringBuilder.Append(streetAddress[i]);
                }
            }
            stringBuilder.Append($",+{zipCode}");
            return stringBuilder.ToString();
        }
    }
}
