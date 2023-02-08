using FilmesApi.Data;
using Microsoft.EntityFrameworkCore;


// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FilmeContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("FilmeConnection"))
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
