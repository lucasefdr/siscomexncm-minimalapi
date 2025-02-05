using APIFindNCM.Domain.Dtos;
using APIFindNCM.Domain;

namespace APIFindNCM.Services;

internal interface INcmService
{
    Task<NcmDtoResponse> GetAll();
    Task<NcmDto> GetByCodNcm(string codNcm);
}
