using Microsoft.EntityFrameworkCore;
using QuickPickDBApi.Models.dbContext_folder;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<QuickPickDbContext>(options =>
{
    var connectionstring = builder.Configuration.GetConnectionString("AzureConnectionString");
    options.UseSqlServer(connectionstring);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.UseSwaggerUI();
//    app.UseSwagger();
//}
app.MapOpenApi();
app.UseSwaggerUI();
app.UseSwagger();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
