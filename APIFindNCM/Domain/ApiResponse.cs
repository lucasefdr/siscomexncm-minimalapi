using System.Text.Json.Serialization;

namespace APIFindNCM.Domain;

internal class ApiResponse
{
    [JsonPropertyName("Data_Ultima_Atualizacao_NCM")]
    public string DataUltimaAtualizacao { get; set; }

    [JsonPropertyName("Ato")]
    public string Ato { get; set; }

    [JsonPropertyName("Nomenclaturas")]
    public List<Ncm> Ncms { get; set; }
}
