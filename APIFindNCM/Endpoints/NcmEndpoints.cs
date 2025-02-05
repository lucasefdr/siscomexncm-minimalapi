using APIFindNCM.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIFindNCM.Endpoints;

internal static class NcmEndpoints
{
    public static void MapNcmEndpoints(this WebApplication app)
    {
        var ncmGroup = app.MapGroup("/ncm"); // Agrupa os endpoints

        app.MapGet("/find-all", async (INcmService ncmService) =>
        {
            var ncms = await ncmService.GetAll();
            return Results.Ok(ncms);
        })
        .WithName("ListaNCMs")
        .WithOpenApi();

        app.MapGet("/find-ncm/{codNcm}", async (INcmService ncmService, [FromRoute] string codNcm) =>
        {
            // Validação do código NCM antes de chamar o serviço
            if (string.IsNullOrWhiteSpace(codNcm) || !codNcm.All(char.IsDigit) || codNcm.Length > 8)
                return Results.BadRequest("O código NCM deve conter apenas números e no máximo 8 dígitos.");

            var ncm = await ncmService.GetByCodNcm(codNcm);
            return ncm is not null ? Results.Ok(ncm) : Results.NotFound("NCM não encontrado.");
        })
        .WithName("BuscaNCMPorCódigo")
        .WithOpenApi();
    }
}
