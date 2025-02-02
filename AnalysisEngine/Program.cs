using AnalysisEngine.Data;
using AnalysisEngine.MessageHandlers;
using FoodQualityAnalysis.Common;
using FoodQualityAnalysis.Common.MessageBroker;
using FoodQualityAnalysis.Common.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
ConfigurationHelper.Initialize(builder.Configuration);
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IMessageConnection, MessageConnection>();
builder.Services.AddTransient<IMessageProducer, MessageProducer>();
builder.Services.AddHostedService<FoodBatchAnalysisConsumer>();
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
using (var serviceScope = app.Services.CreateScope())
{
    var messageConnection = serviceScope.ServiceProvider.GetService<IMessageConnection>();
    await messageConnection.InitializeConnection();
    var context = serviceScope.ServiceProvider.GetService<DataContext>();
    await context.Database.MigrateAsync();
}
await app.RunAsync();