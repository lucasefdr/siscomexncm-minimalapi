using APIFindNCM.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIFindNCM.Endpoints;

internal static class NcmEndpoints
{
    public static void MapNcmEndpoints(this WebApplication app)
    {
        app.MapGet("/find-all", async (INcmService ncmService) =>
        {
            var ncms = await ncmService.GetAll();
            return Results.Ok(ncms);
        })
        .WithName("ListaNCMs")
        .WithOpenApi();

        app.MapGet("/find-ncm/{codNcm:int}", async (INcmService ncmService, [FromRoute] string codNcm) =>
        {
            var ncm = await ncmService.GetByCodNcm(codNcm);
            return ncm is not null ? Results.Ok(ncm) : Results.NotFound();
        })
        .WithName("BuscaNCMPorCódigo")
        .WithOpenApi();
    }
}
