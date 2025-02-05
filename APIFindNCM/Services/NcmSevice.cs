using APIFindNCM.Domain.Dtos;
using APIFindNCM.Domain;
using System.Text.Json;

namespace APIFindNCM.Services;

internal class NcmService : INcmService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string baseAddress = @"https://portalunico.siscomex.gov.br/classif/api/publico/nomenclatura/download/json?perfil=PUBLICO";

    public NcmService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<NcmDto> GetByCodNcm(string codNcm)
    {
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri(baseAddress);

        var response = await client.GetAsync(baseAddress);

        if (response.IsSuccessStatusCode)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(stringResponse);

            var ncm = apiResponse?.Ncms
                .FirstOrDefault(ncm => ncm.Codigo.Replace(".", "") == codNcm);

            var result = new NcmDto
            {
                Codigo = ncm.Codigo,
                Descricao = ncm.Descricao,
                DataInicioValidade = ncm.DataInicio,
                DataFimValidade = ncm.DataFim
            };

            return result;
        }
        else
        {
            throw new HttpRequestException(response.ReasonPhrase);
        }


    }

    public async Task<NcmDtoResponse> GetAll()
    {
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri(baseAddress);

        var response = await client.GetAsync(baseAddress);
        var result = new List<NcmDto>();

        if (response.IsSuccessStatusCode)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(stringResponse);

            result = apiResponse?.Ncms
                .Where(ncm => ncm.Codigo.Length == 10)
                .Select(ncm => new NcmDto
                {
                    Codigo = ncm.Codigo.Replace(".", ""),
                    Descricao = ncm.Descricao,
                    DataInicioValidade = ncm.DataInicio,
                    DataFimValidade = ncm.DataFim
                }).ToList();
        }
        else
        {
            throw new HttpRequestException(response.ReasonPhrase);
        }

        return new NcmDtoResponse
        {
            Total = result.Count,
            NcmList = result,
        };
    }


}
