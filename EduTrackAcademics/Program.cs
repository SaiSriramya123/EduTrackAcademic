using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EduTrackAcademics.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EduTrackAcademicsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EduTrackAcademicsContext") ?? throw new InvalidOperationException("Connection string 'EduTrackAcademicsContext' not found.")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
	app.UseSwaggerUI(options => {
		options.SwaggerEndpoint("/openapi/v1.json", "Education API v1");
		options.RoutePrefix = "swagger";
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
