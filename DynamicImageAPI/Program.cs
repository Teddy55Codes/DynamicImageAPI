using System.Reflection;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.{env.EnvironmentName}.json",
    optional: true,
    reloadOnChange: true);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

var app = builder.Build();

// Set-up sqlite db
var imagesDb = SQLiteDBAccess.SQLiteDBAccess.Instance("Images", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

imagesDb.CreateTable("Counters", "Id INTEGER PRIMARY KEY, Name TEXT, Count INTEGER", false);
imagesDb.CreateTable("Locations", "Query TEXT PRIMARY KEY, City TEXT, IsMobile INTEGER, IsProxy INTEGER", false);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();