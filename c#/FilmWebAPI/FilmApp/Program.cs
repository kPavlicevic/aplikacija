using FilmRecenzijaApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sgo => { // sgo je instanca klase SwaggerGenOptions
    var o = new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Recenzija filmova API",
        Version = "v1",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "katarina.pavlicevic5@gmail.com",
            Name = "Katarina Pavlièeviæ"
        },
        Description = "Ovo je recenzija na filmove",
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "Filmska licenca"
        }
    };
    sgo.SwaggerDoc("v1", o);


});

//dodavnje baze podataka

builder.Services.AddDbContext<FilmRecenzijaContext> (o =>
    o.UseSqlServer(builder.Configuration.
        GetConnectionString(name: "FilmRecenzijaContext")
        )
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opcije =>
    {
        opcije.SerializeAsV2 = true;
    });
    app.UseSwaggerUI(opcije =>
    {
        opcije.ConfigObject.AdditionalItems.Add("requestSnippetsEnabled", true);
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
