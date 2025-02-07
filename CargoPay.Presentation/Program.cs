using CargoPay.Application.Services;
using CargoPay.Application.Services.Interfaces;
using CargoPay.Infrastructure.Data;
using CargoPay.Infrastructure.Repositories;
using CargoPay.Infrastructure.Repositories.Interfaces;
using CargoPay.Presentation.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(
        AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CargoPay API", Version = "v1" });

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Introduce tus credenciales (username:password) codificadas en Base64."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CargoPayConnection")));

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardService, CardService>();

builder.Services.AddSingleton<FeeUpdateService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<FeeUpdateService>());
builder.Services.AddSingleton<IPaymentFeeService>(provider =>
{
    var feeUpdateService = provider.GetRequiredService<FeeUpdateService>();
    UniversalFeesExchange.Initialize(feeUpdateService);
    return UniversalFeesExchange.Instance;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CargoPay API v1"));
}
app.UseMiddleware<BasicAuthMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();