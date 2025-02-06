using APIFindNCM.Domain.Dtos;
using APIFindNCM.Domain.Enums;
using APIFindNCM.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIFindNCM.Endpoints;

internal static class NcmEndpoints
{
    public static void MapNcmEndpoints(this WebApplication app)
    {
        var ncmGroup = app.MapGroup("/ncm"); // Agrupa os endpoints

        app.MapGet("/find-all", async (INcmService ncmService, [FromQuery] NcmNivel? nivel, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
        {
            NcmDtoResponse ncms = await ncmService.GetNcmByNivel(nivel, page, pageSize);
            return Results.Ok(ncms);
        })
        .WithName("ListaNCMs")
        .WithOpenApi();

        app.MapGet("/find-ncm/{codNcm}", async (INcmService ncmService, [FromRoute] string codNcm) =>
        {
            // Validação do código NCM antes de chamar o serviço
            if (string.IsNullOrWhiteSpace(codNcm) || !codNcm.All(char.IsDigit) || codNcm.Length > 8)
                return Results.BadRequest("O código NCM deve conter apenas números e no máximo 8 dígitos.");

            var ncm = await ncmService.GetNcmByCodigo(codNcm);
            return ncm is not null ? Results.Ok(ncm) : Results.NotFound("NCM não encontrado.");
        })
        .WithName("BuscaNCMPorCódigo")
        .WithOpenApi();
    }
}
