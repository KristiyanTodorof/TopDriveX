using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TopDriveX.Infrastructure.ExternalServices.Nhtsa;

namespace TopDriveX.Infrastructure.ExternalServices
{
    public class NhtsaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NhtsaService> _logger;
        private const string BaseUrl = "https://vpic.nhtsa.dot.gov/api/vehicles";

        public NhtsaService(HttpClient httpClient, ILogger<NhtsaService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<NhtsaMake>> GetAllMakesAsync()
        {
            try
            {
                var url = $"{BaseUrl}/GetAllMakes?format=json";
                _logger.LogInformation("Fetching makes from NHTSA API: {Url}", url);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<NhtsaResponse<NhtsaMake>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Successfully fetched {Count} makes", result?.Results.Count ?? 0);
                return result?.Results ?? new List<NhtsaMake>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching makes from NHTSA API");
                return new List<NhtsaMake>();
            }
        }

        public async Task<List<NhtsaModel>> GetModelsForMakeAsync(int makeId)
        {
            try
            {
                var url = $"{BaseUrl}/GetModelsForMakeId/{makeId}?format=json";
                _logger.LogInformation("Fetching models for make {MakeId}", makeId);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<NhtsaResponse<NhtsaModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Results ?? new List<NhtsaModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching models for make {MakeId}", makeId);
                return new List<NhtsaModel>();
            }
        }

        public async Task<List<NhtsaVehicleType>> GetVehicleTypesAsync()
        {
            try
            {
                // Get all vehicle types
                var url = $"{BaseUrl}/GetVehicleTypesForMake/car?format=json";
                _logger.LogInformation("Fetching vehicle types from NHTSA API");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<NhtsaResponse<NhtsaVehicleType>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Successfully fetched {Count} vehicle types", result?.Results.Count ?? 0);
                return result?.Results ?? new List<NhtsaVehicleType>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching vehicle types from NHTSA API");
                return new List<NhtsaVehicleType>();
            }
        }
    }
}