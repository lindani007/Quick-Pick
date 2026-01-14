using QuickPickSignlaRApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.MapOpenApi();
////}
app.MapOpenApi();
//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<AisleHub>("/aisleHub");
app.MapHub<ItemHub>("/itemHub");
app.MapHub<OrderHub>("/orderHub");
app.MapHub<StatusHub>("/statusHub");

app.MapHub<TransactionHub>("/transactionHub");
app.MapHub<OrderedItemHub>("/orderedItemHub");
app.MapControllers();

app.Run();
