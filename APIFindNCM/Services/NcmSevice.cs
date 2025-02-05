using APIFindNCM.Domain.Dtos;
using APIFindNCM.Domain;
using System.Text.Json;

namespace APIFindNCM.Services;

internal class NcmService : INcmService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string BaseUrl = "https://portalunico.siscomex.gov.br/classif/api/publico/nomenclatura/download/json?perfil=PUBLICO";

    public NcmService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<NcmDto?> GetByCodNcm(string codNcm)
    {
        var apiResponse = await FetchNcmDataAsync();

        if (apiResponse?.Ncms == null || apiResponse.Ncms.Count == 0)
            return null;

        var ncm = apiResponse.Ncms.FirstOrDefault(ncm => ncm.Codigo.Replace(".", "") == codNcm);

        return ncm is null ? null : MapToDto(ncm);
    }



    public async Task<NcmDtoResponse> GetAll()
    {
        var apiResponse = await FetchNcmDataAsync();

        var result = apiResponse?.Ncms
            .Where(ncm => ncm.Codigo.Length == 10)
            .Select(MapToDto)
            .ToList() ?? [];

        return new NcmDtoResponse
        {
            Total = result.Count,
            NcmList = result
        };
    }

    private async Task<ApiResponse?> FetchNcmDataAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(BaseUrl);

            // Valida se a retornou um status code de sucesso, se não lança exception 
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(stringResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    private NcmDto? MapToDto(Ncm ncm)
    {
        return new NcmDto
        {
            Codigo = ncm.Codigo.Replace(".", ""),
            Descricao = ncm.Descricao,
            DataInicioValidade = ParseData(ncm.DataInicio),
            DataFimValidade = ParseData(ncm.DataFim)
        };
    }

    private static string ParseData(string data)
    {
        if (DateTime.TryParse(data, out var parsedDate))
        {
            return parsedDate.ToString("dd/MM/yyyy");
        }
        return data; // Retorna a string original se não puder converter
    }
}
