using Microsoft.EntityFrameworkCore;
using TaskManager.Adapters.Adapters.Task;
using TaskManager.Adapters.Persistence;
using TaskManager.Adapters.Security;
using TaskManager.API.Facades;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Security;

namespace TaskManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TaskManager",
                    Version = "v1",
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Informe o token JWT:",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //banco de dados InMemory
            /*builder.Services.AddDbContext<DbContextInMemory>(options =>
                options.UseInMemoryDatabase("DbContextInMemory"));*/

            //Banco de dados SQL
            /*var cnn = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Conexão: {cnn}");*/

            builder.Services.AddDbContext<DbContextTaskManager>(options => options
            .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            //Facades
            builder.Services.AddScoped<TaskUseCaseFacade>();
            builder.Services.AddScoped<TaskCategoryUseCaseFacade>();
            builder.Services.AddScoped<UserUseCaseFacade>();

            //Ports
            builder.Services.AddScoped<ICreateTaskPort, CreateTaskAdapter>();
            builder.Services.AddScoped<IDeleteTaskPort, DeleteTaskAdapter>();
            builder.Services.AddScoped<IGetTaskByIdPort, GetTaskByIdAdapter>();
            builder.Services.AddScoped<ISearchTaskPort, SearchTaskAdapter>();
            builder.Services.AddScoped<IUpdateTaskDetailsPort, UpdateTaskDetailsAdapter>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ICurrentUserPort, HttpCurrentUserAdapter>();

   


            //builder.Services.AddTransient<>(); MAPPERS ou Helpers

            //Autenticação JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            //Autorização
            builder.Services.AddAuthorization();

            var app = builder.Build();

            //pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API v1");
                });
            }


            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
