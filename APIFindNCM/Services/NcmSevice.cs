using APIFindNCM.Domain.Dtos;
using APIFindNCM.Domain;
using System.Text.Json;
using APIFindNCM.Domain.Enums;

namespace APIFindNCM.Services;

internal class NcmService : INcmService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string BaseUrl = "https://portalunico.siscomex.gov.br/classif/api/publico/nomenclatura/download/json?perfil=PUBLICO";

    public NcmService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<NcmDto?> GetNcmByCodigo(string codNcm)
    {
        var apiResponse = await FetchNcmDataAsync();

        if (apiResponse?.Ncms == null || apiResponse.Ncms.Count == 0)
            return null;

        var ncm = apiResponse.Ncms.FirstOrDefault(ncm => ncm.Codigo.Replace(".", "") == codNcm);

        return ncm is null ? null : MapToDto(ncm);
    }

    public async Task<NcmDtoResponse> GetNcmByNivel(NcmNivel? nivel, int page, int pageSize)
    {
        var apiResponse = await FetchNcmDataAsync();

        var query = apiResponse?.Ncms.AsQueryable() ?? Enumerable.Empty<Ncm>().AsQueryable();

        // Filtra sem materializar a coleção (mantém como IQueryable)
        query = query.Where(ncm => FilterByNivel(ncm.Codigo, nivel));

        int totalFiltered = query.Count(); // Conta antes de paginar

        var paginatedResult = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToDto) // Transforma apenas os dados paginados
            .ToList();

        return new NcmDtoResponse
        {
            Total = totalFiltered,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalFiltered / pageSize),
            NcmList = paginatedResult
        };
    }

    private static bool FilterByNivel(string codigo, NcmNivel? nivel)
    {
        var codigoNcm = codigo.Replace(".", "");

        return nivel switch
        {
            NcmNivel.Capitulo => codigoNcm.Length == 2,
            NcmNivel.Posicao => codigoNcm.Length == 4,
            NcmNivel.SubPosicao => codigoNcm.Length == 6,
            NcmNivel.Item => codigoNcm.Length == 7,
            NcmNivel.SubItem => codigoNcm.Length == 8,
            _ => true
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
