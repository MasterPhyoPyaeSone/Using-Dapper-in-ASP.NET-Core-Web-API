using System;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// 1. Connection String ကို ဖတ်ယူခြင်း
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. IDbConnection ကို Singleton သို့မဟုတ် Scoped အနေဖြင့် Register လုပ်ခြင်း
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));


// 2. Add services to the container.
builder.Services.AddControllers();          // ၎င်းမပါလျှင် သင်ရေးထားသော Controller များကို API အနေနဲ့ ခေါ်ယူ၍ မရနိုင်ပါ။
builder.Services.AddEndpointsApiExplorer(); // /api/Product) ကို ရှာဖွေဖော်ထုတ်ပေးသည့် စနစ်ဖြစ်သည်။
builder.Services.AddSwaggerGen();           // Generates the Swagger UI documentation
builder.Services.AddOpenApi(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Adds the visual UI at /swagger
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers(); // ဤလိုင်းရှိနေဖို့ လိုအပ်ပါတယ်

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
