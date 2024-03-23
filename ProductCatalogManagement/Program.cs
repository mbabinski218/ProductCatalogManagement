using ProductCatalogManagement.ExceptionHandlers;
using ProductCatalogManagement.Infrastructure;
using ProductCatalogManagement.GraphQl;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var logging = builder.Logging;
const string cors = "cors";

logging.ClearProviders();
var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
logging.AddSerilog(logger);

services.AddCors(options => options.AddPolicy(cors, corsBuilder => corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
services.AddExceptionHandlers();
services.AddInfrastructure(configuration);
services.AddGraphQl();

var app = builder.Build();

app.UseCors(cors);
app.UseExceptionHandler(_ => { });
app.MapGraphQl();
app.UseHttpsRedirection();

app.SeedDatabase();

app.Run();