using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;
using IOB___Controle_de_Saldo_Bancario.Service;
using IOB___Controle_de_Saldo_Bancario.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.Configure<AwsOptions>(builder.Configuration.GetSection("Aws"));
builder.Services.AddScoped<IAmazonSQS, AmazonSQSClient>(provider =>
{
    var options = provider.GetRequiredService<IOptions<AwsOptions>>().Value;
    var config = new AmazonSQSConfig { RegionEndpoint = RegionEndpoint.USEast1 };
    var credentials = new BasicAWSCredentials(options.AccessKey, options.SecretKey);
    return new AmazonSQSClient(credentials, config);
});

var CorsName = "namecors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsName, policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

builder.Services.AddDbContext<DbContextBank>(options =>
{
    var ConnectionString = builder.Configuration.GetConnectionString("mysql");
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<BankAccount>, BankAccountRepository>();
builder.Services.AddScoped<IRepository<BankLaunch>, BankLaunchRepository>();
builder.Services.AddScoped<IServicoBancario, ServicoBancario>();
builder.Services.AddScoped<ISqsMessageProcessor, SqsMessageProcessor>();
builder.Services.AddScoped<ISqsMessageRetryHandler, SqsMessageRetryHandler>();
builder.Services.AddScoped<ISqsMessagePoller, SqsMessagePoller>();
builder.Services.AddScoped<SqsConsumerService>();
builder.Services.AddHostedService(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    var scope = scopeFactory.CreateScope();
    return scope.ServiceProvider.GetRequiredService<SqsConsumerService>();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(CorsName);
app.UseAuthorization();
app.MapControllers();
app.Run();
