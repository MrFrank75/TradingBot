
using TradingBot.BinanceServices;

namespace TradingBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IBinanceConnectorWrapper, BinanceConnectorWrapper>();

            var app = builder.Build();

            //forced to be development for now
            app.Environment.EnvironmentName = "Development";

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapControllers();
            app.MapGet("/Environment", () =>
            {
                return new EnvironmentInfo();
            });

            app.Run();
        }
    }
}