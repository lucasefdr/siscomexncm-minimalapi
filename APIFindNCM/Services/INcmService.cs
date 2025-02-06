using APIFindNCM.Domain.Dtos;
using APIFindNCM.Domain;
using APIFindNCM.Domain.Enums;

namespace APIFindNCM.Services;

internal interface INcmService
{
    Task<NcmDto?> GetNcmByCodigo(string codNcm);

    Task<NcmDtoResponse> GetNcmByNivel(NcmNivel? nivel, int page, int pageSize);
}
