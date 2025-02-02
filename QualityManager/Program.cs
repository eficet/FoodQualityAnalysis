using FoodQualityAnalysis.Common;
using FoodQualityAnalysis.Common.MessageBroker;
using FoodQualityAnalysis.Common.Middlewares;
using FoodQualityAnalysis.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using QualityManager.AutoMapper;
using QualityManager.Data;
using QualityManager.Interfaces;
using QualityManager.MessageHandlers;
using QualityManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
ConfigurationHelper.Initialize(builder.Configuration);
builder.Services.AddDbContext<QualityManagerContext>(options=> options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")) );
builder.Services.AddSingleton<IMessageConnection, MessageConnection>();
builder.Services.AddScoped<IMessageProducer, MessageProducer>();
builder.Services.AddScoped<IFoodQualityService, FoodQualityService>();
builder.Services.AddHostedService<FoodQualityAnalysisResponseConsumer>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(FoodBatchProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

using (var serviceScope = app.Services.CreateScope())
{
    var messageConnection = serviceScope.ServiceProvider.GetService<IMessageConnection>();
    await messageConnection.InitializeConnection();
    
    var context = serviceScope.ServiceProvider.GetService<QualityManagerContext>();
    await context.Database.MigrateAsync();
}
await app.RunAsync();