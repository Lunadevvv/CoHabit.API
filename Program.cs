
using System.Text;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Implements;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Implements;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CoHabit.API.Helpers;

namespace CoHabit.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "CoHabit API", Version = "v1" });

                // Add JWT bearer authentication to Swagger
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT Bearer token **_only_**"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            
            builder.Services.AddDbContext<CoHabitDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<CoHabitDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ModeratorPolicy", policy => policy.RequireRole("Moderator"));
                options.AddPolicy("ProPolicy", policy => policy.RequireRole("ProMember"));
                options.AddPolicy("PlusPolicy", policy => policy.RequireRole("PlusMember"));
                options.AddPolicy("BasicPolicy", policy => policy.RequireRole("BasicMember"));
            });

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JwtOptions:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]!)),
                        ValidateIssuerSigningKey = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Lấy token từ cookie thay vì header
                            context.Token = context.Request.Cookies["AccessToken"];
                            return Task.CompletedTask;
                        }
                    };
                });

            //CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins(
                            "https://cohabit-dun.vercel.app",
                            "https://cohabit-d134fu1op-huyld1504s-projects.vercel.app",
                            "https://cohabit-git-main-huyld1504s-projects.vercel.app",
                            "http://localhost:5173",
                            "https://localhost:5173"
                        )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials(); // QUAN TRỌNG: Cho phép cookies
                    });
            });

            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOtpRepository, OtpRepository>();
            builder.Services.AddScoped<IOtpService, OtpService>();
            builder.Services.AddScoped<ICharacteristicRepository, CharacteristicRepository>();
            builder.Services.AddScoped<ICharacteristicService, CharacteristicService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IFurnitureRepository, FurnitureRepository>();
            builder.Services.AddScoped<IFurnitureService, FurnitureService>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ISubcriptionRepository, SubcriptionRepository>();
            builder.Services.AddScoped<ISubcriptionService, SubcriptionService>();
            builder.Services.AddScoped<IUserSubcriptionRepository, UserSubcriptionRepository>();
            builder.Services.AddScoped<IUserSubcriptionService, UserSubcriptionService>();

            // PayOS configuration and HttpClient
            builder.Services.Configure<PayOSConfig>(builder.Configuration.GetSection("PayOS"));
            builder.Services.AddHttpClient("payos", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["PayOS:BaseUrl"] ?? "https://api-merchant.payos.vn/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Brevo configuration and HttpClient
            builder.Services.Configure<BrevoConfig>(builder.Configuration.GetSection("Brevo"));
            builder.Services.AddHttpClient("brevo", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Brevo:BaseUrl"] ?? "https://api.brevo.com/v3/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Cloudinary configuration
            builder.Services.Configure<CloudinaryConfig>(builder.Configuration.GetSection("Cloudinary"));

            builder.Services.AddScoped<IPayOSService, PayOSService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
