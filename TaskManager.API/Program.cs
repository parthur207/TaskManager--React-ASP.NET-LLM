using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;
using TaskManager.Adapters.DI;
using TaskManager.Adapters.Persistence;
using TaskManager.API.Facades.DI;
using TaskManager.API.Hubs.RealTime;
using TaskManager.API.Realtime;
using TaskManager.Core.DI;
using TaskManager.Core.Enums;
using TaskManager.Core.ResponsePattern;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();

builder.Services.AddRealtime(detailedErrors: builder.Environment.IsDevelopment());

builder.Services.AddOpenApiDocument(options =>
{
    options.Title = "TaskManager API";
    options.Version = "v1";
    options.Description = "API de gerenciamento de tarefas";

    options.AddSecurity("Bearer", new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Insira o token JWT no campo abaixo."
    });

    options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

// ---------------------------------------------------------------- JWT
// Falha no startup (e não na primeira requisição) se a chave estiver ausente ou fraca.
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32)
    throw new InvalidOperationException("Jwt:Key não configurado ou menor que 32 bytes (defina via variável de ambiente JWT_KEY / Jwt__Key).");

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };

    // WebSocket do navegador não envia header Authorization: o SignalR manda o token na query string.
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken) &&
                context.HttpContext.Request.Path.StartsWithSegments(RealtimeExtensions.HubPath))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ---------------------------------------------------------------- CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (allowedOrigins is null || allowedOrigins.Length == 0)
    allowedOrigins = ["http://localhost:5173"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()); // necessário para o SignalR
});

// ---------------------------------------------------------------- Camadas
builder.Services.AddCore();
builder.Services.AddAdapters(builder.Configuration);
builder.Services.AddFacades();

var app = builder.Build();

// ---------------------------------------------------------------- Pipeline
// Primeiro da fila: captura qualquer exceção não tratada (ex.: ArgumentException dos VOs/entidades)
// e responde no mesmo formato { status, message } que os controllers já usam.
app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GlobalException");

    var response = new SimpleResponseModel();

    if (exception is ArgumentException) // regra de domínio violada (senha fraca, status inválido, nome vazio...)
    {
        logger.LogWarning(exception, "Requisição inválida.");
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        response.Status = ResponseStatusEnum.Error;
        response.Message = exception.Message;
    }
    else
    {
        logger.LogError(exception, "Erro não tratado.");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        response.Status = ResponseStatusEnum.CriticalError;
        response.Message = "Ocorreu um erro inesperado.";
    }

    await context.Response.WriteAsJsonAsync(response);
}));

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
    app.UseOpenApi();
    app.UseSwaggerUi(settings =>
    {
        settings.DocumentTitle = "TaskManager API";
        settings.Path = "/swagger";
    });
}

// No Docker o container só escuta HTTP (8080): defina UseHttpsRedirection=false no compose.
if (builder.Configuration.GetValue("UseHttpsRedirection", true))
    app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRealtime();
app.MapHealthChecks("/health").AllowAnonymous();

// ---------------------------------------------------------------- Migrations (opt-in)
// Sem isso o banco TaskManagerDb do container nunca é criado.
if (builder.Configuration.GetValue<bool>("Database:MigrateOnStartup"))
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<DbContextTaskManager>().Database.MigrateAsync();
}

app.Run();