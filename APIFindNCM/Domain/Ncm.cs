using System.Text.Json.Serialization;

namespace APIFindNCM.Domain;

internal class Ncm
{
    [JsonPropertyName("Codigo")]
    public string Codigo { get; set; }

    [JsonPropertyName("Descricao")]
    public string Descricao { get; set; }

    [JsonPropertyName("Data_Inicio")]
    public string DataInicio { get; set; }

    [JsonPropertyName("Data_Fim")]
    public string DataFim { get; set; }

    [JsonPropertyName("Tipo_Ato_Ini")]
    public string TipoAto { get; set; }

    [JsonPropertyName("Numero_Ato_Ini")]
    public string NumeroAto { get; set; }

    [JsonPropertyName("Ano_Ato_Ini")]
    public string AnoAto { get; set; }
}