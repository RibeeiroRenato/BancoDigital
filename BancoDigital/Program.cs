using BancoDidital.Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BancoDigital.Application.Interface;
using BancoDigital.Application.Repository;
using BancoDigital.Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var contaCorrenteConnection = builder.Configuration.GetConnectionString("ContaCorrenteConnection");
var tarifaConnection = builder.Configuration.GetConnectionString("TarifasConnection");
var transferenciaConnection = builder.Configuration.GetConnectionString("TransferenciaConnection");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar DbContexts (AddDbContext já registra como Scoped por padrão)
builder.Services.AddDbContext<contaCorrenteContext>(options =>
    options.UseSqlServer(contaCorrenteConnection));

builder.Services.AddDbContext<tarifaContext>(options =>
    options.UseSqlServer(tarifaConnection));

builder.Services.AddDbContext<transferenciaContext>(options =>
    options.UseSqlServer(transferenciaConnection));
// Serviços e repositórios (scoped é apropriado)
builder.Services.AddScoped<IContaCorrente, ContaCorrenteService>();
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ITransfereciaRepository, transferenciaRepository>();
builder.Services.AddScoped<ITransferencia, TransferenciaService>();
builder.Services.AddHttpClient(); // Add this line before your repository registrations

// Move this block before builder.Build()
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<contaCorrenteContext>();
    db.Database.CanConnect();// Retorna true se conectar
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();