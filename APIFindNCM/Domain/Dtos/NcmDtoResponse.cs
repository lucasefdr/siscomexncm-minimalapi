namespace APIFindNCM.Domain.Dtos;

internal class NcmDtoResponse
{
    public int Total { get; set; } // Total de registros disponíveis
    public int Page { get; set; } // Página atual
    public int PageSize { get; set; } // Quantidade por página
    public int TotalPages { get; set; } // Total de páginas
    public List<NcmDto> NcmList { get; set; } = [];

}
