using PassKeeper.Data;

namespace PassKeeper.AppWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthorization();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddDataModule(builder.Configuration);

        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MapControllerRoute(name: "default", pattern: "api/{controller}/{action}");
        app.Run();
    }
}