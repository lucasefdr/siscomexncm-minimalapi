using APIFindNCM.Services;

namespace APIFindNCM.Extensions;

public static class BuilderExtensions
{
    public static void AddArchitectures(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<INcmService, NcmService>();
    }


}
