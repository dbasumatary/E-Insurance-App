using Microsoft.Extensions.DependencyInjection;
using eInsuranceApp.Business_Layer.Implementation;
using eInsuranceApp.Business_Layer.Interface;
using Microsoft.IdentityModel.Tokens;
using eInsuranceApp.RepositoryLayer.Interface;
using eInsuranceApp.RepositoryLayer.Service;
using RabbitMQ.Client;
using eInsuranceApp.DBContext;
using Microsoft.EntityFrameworkCore;
using eInsuranceApp.Email;
using eInsuranceApp.RepositoryLayer.Implementation;
using eInsuranceApp.UnitOfWork;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace eInsuranceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register EmailSettings from appsettings.json
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            //builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

            // MsSql
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Jwt
            //var jwtKey = Environment.GetEnvironmentVariable("JwtKey");
            //var jwtKey = builder.Configuration["JwtKey"];
            var jwtKey = builder.Configuration.GetSection("JwtSettings")["JwtKey"];
            //var decodedKey = Convert.FromBase64String(jwtKey);
            //var decodedKey = Encoding.ASCII.GetBytes(jwtKey);
            var decodedKey = Encoding.UTF8.GetBytes(jwtKey);

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT key is missing in the configuration.");
            }
            //var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            var key = Encoding.ASCII.GetBytes(jwtKey);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(decodedKey),
                    ValidateIssuer = false,
                    //ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidateAudience = false,
                    //ValidAudience = builder.Configuration["JwtSettings:Audience"]
                    ValidateLifetime = true
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Token failed validation: " + context.Exception);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated successfully");
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //Swagger Jwt
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "eInsuranceApp API", Version = "v1" });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {
                    new OpenApiSecurityScheme
                    {
                      Reference = new OpenApiReference
                      {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                      }
                    },
                    new string[]{}
                  }
                });
            });

            builder.Services.AddTransient<IAdminRL, AdminRL>();
            builder.Services.AddTransient<IAdminBL, AdminBL>();
            builder.Services.AddTransient<IEmailProducer, EmailProducer>();
            builder.Services.AddTransient<IEmailConsumer, EmailConsumer>();
            builder.Services.AddTransient<IEmployeeRL, EmployeeRL>();
            builder.Services.AddTransient<IEmployeeBL, EmployeeBL>();
            builder.Services.AddTransient<IAgentRL, AgentRL>();
            builder.Services.AddTransient<IAgentBL, AgentBL>();
            builder.Services.AddTransient<ICustomerRL, CustomerRL>();
            builder.Services.AddTransient<ICustomerBL, CustomerBL>();

            builder.Services.AddTransient<IPolicyRL, PolicyRL>();
            builder.Services.AddTransient<IPolicyBL, PolicyBL>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWorkImplement>();

            builder.Services.AddTransient<ILoginRL, LoginRL>();
            builder.Services.AddTransient<ILoginBL, LoginBL>();

            builder.Services.AddTransient<IPremiumRL, PremiumRL>();
            builder.Services.AddTransient<IPremiumBL, PremiumBL>();
            builder.Services.AddTransient<IPaymentRL, PaymentRL>();
            builder.Services.AddTransient<IPaymentBL, PaymentBL>();

            builder.Services.AddTransient<ICommissionRL, CommissionRL>();
            builder.Services.AddTransient<ICommissionBL, CommissionBL>();


            //builder.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
            //    options.InstanceName = builder.Configuration["RedisCacheOptions:InstanceName"];
            //});

            //builder.Services.AddSingleton<EmailSender>();

            // RabbitMQ connection
            //builder.Services.AddSingleton<IConnection>(sp =>
            //{
            //    var factory = new ConnectionFactory { HostName = "localhost" };
            //    var connection = factory.CreateConnectionAsync();
            //    //return factory.CreateConnection();
            //    return (IConnection)connection;
            //});

            builder.Services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost", 
                    Port = 5672,            
                    UserName = "guest", 
                    Password = "guest" 
                };
                return factory.CreateConnection();
            });

            var app = builder.Build();

            

            //app.MapGet("/", async (IEmailSender emailSender) =>
            //{
            //    await emailSender.SendEmailAsync("test@example.com", "Test Subject", "Test Body");
            //    return "Email sent!";
            //});

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Start consuming email messages
            //var emailConsumer = app.Services.GetRequiredService<IEmailConsumer>();
            //emailConsumer.StartConsuming();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}
