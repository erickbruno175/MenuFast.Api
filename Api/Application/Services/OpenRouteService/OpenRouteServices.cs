using System.Globalization;
using System.Text.Json;

namespace MenuFast.Api.Api.Application.Services.Services.OpenRouteService {
    public class OpenRouteServices {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenRouteServices(
            HttpClient httpClient,
            IConfiguration configuration) {
            _httpClient = httpClient;
            _configuration = configuration;
        }

      
        public async Task<(decimal Latitude, decimal Longitude)?> BuscarCoordenadasAsync(string cep,string logradouro,string numero,string bairro,string cidade,string estado) {
            var apiKey = _configuration [ "OpenRouteService:ApiKey" ];

            if(string.IsNullOrWhiteSpace(apiKey))throw new InvalidOperationException("OpenRouteService API Key não configurada.");

            var endereco = string.Join(", ",
                new [ ]{logradouro,numero,bairro,cidade,estado,cep,"Brasil"}.Where(x => !string.IsNullOrWhiteSpace(x)));

            var url =
                "https://api.heigit.org/pelias/v1/search" +
                $"?api_key={Uri.EscapeDataString(apiKey)}" +
                $"&text={Uri.EscapeDataString(endereco)}";

            var response = await _httpClient.GetAsync(url);

            if(!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);

            if(!document.RootElement.TryGetProperty("features",out var features))return null;

            if(features.GetArrayLength() == 0)return null;

            var coordinates = features [ 0 ].GetProperty("geometry").GetProperty("coordinates");
            var longitude = coordinates [ 0 ].GetDecimal();
            var latitude = coordinates [ 1 ].GetDecimal();

            return (latitude, longitude);
        }


        public async Task<decimal?> CalcularDistanciaAsync(decimal latitudeOrigem,decimal longitudeOrigem,decimal latitudeDestino,decimal longitudeDestino) {
            var apiKey = _configuration [ "OpenRouteService:ApiKey" ];

            if(string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("OpenRouteService API Key não configurada.");

            var cultura = CultureInfo.InvariantCulture;

            var url =
                "https://api.openrouteservice.org/v2/directions/driving-car" +
                $"?api_key={Uri.EscapeDataString(apiKey)}" +
                $"&start={longitudeOrigem.ToString(cultura)},{latitudeOrigem.ToString(cultura)}" +
                $"&end={longitudeDestino.ToString(cultura)},{latitudeDestino.ToString(cultura)}";

            var response = await _httpClient.GetAsync(url);

            if(!response.IsSuccessStatusCode)return null;

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);

            if(!document.RootElement.TryGetProperty("features",out var features))return null;

            if(features.GetArrayLength() == 0)return null;

            var distanciaMetros = features [ 0 ].GetProperty("properties").GetProperty("summary").GetProperty("distance").GetDecimal();
            // Retorna em quilômetros
            return distanciaMetros / 1000m;
        }
    }
}