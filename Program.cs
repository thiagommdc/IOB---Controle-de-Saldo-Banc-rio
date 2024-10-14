using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;
using IOB___Controle_de_Saldo_Bancario.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .WriteTo.MongoDB("string mongo", collectionName: "logs")
//    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

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
