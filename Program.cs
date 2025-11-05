
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
using CloudinaryDotNet;
using CoHabit.API.Hubs;

namespace CoHabit.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load .env file
            DotNetEnv.Env.Load();

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

            builder.Services.AddDbContext<CoHabitDbContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")));

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
                        ValidIssuer = Environment.GetEnvironmentVariable("JwtOptions__Issuer"),
                        ValidateAudience = true,
                        ValidAudience = Environment.GetEnvironmentVariable("JwtOptions__Audience"),
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JwtOptions__Secret")!)),
                        ValidateIssuerSigningKey = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Lấy token từ cookie thay vì header
                            context.Token = context.Request.Cookies["AccessToken"];
                            
                            // Hỗ trợ SignalR authentication qua query string
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                            {
                                context.Token = accessToken;
                            }
                            
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
                            "https://localhost:5173",
                            "https://cohabit.vn",
                            "http://cohabit.vn"
                        )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials() // QUAN TRỌNG: Cho phép cookies
                            .SetIsOriginAllowedToAllowWildcardSubdomains(); // Hỗ trợ SignalR
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
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<IPayOSService, PayOSService>();
            builder.Services.AddScoped<IPostFeedbackRepository, PostFeedbackRepository>();
            builder.Services.AddScoped<IPostFeedbackService, PostFeedbackService>();
            builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();

            // PayOS HttpClient
            builder.Services.AddHttpClient("payos", client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("PayOS__BaseUrl") ?? "https://api-merchant.payos.vn/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Brevo HttpClient
            builder.Services.AddHttpClient("brevo", client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("Brevo__BaseUrl") ?? "https://api.brevo.com/v3/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Cloudinary configuration
            var cloudinaryAccount = new Account(
                Environment.GetEnvironmentVariable("Cloudinary__CloudName"),
                Environment.GetEnvironmentVariable("Cloudinary__ApiKey"),
                Environment.GetEnvironmentVariable("Cloudinary__ApiSecret")
            );

            var cloudinary = new Cloudinary(cloudinaryAccount);
            builder.Services.AddSingleton(cloudinary);

            //SignalR with CORS support
            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

            var app = builder.Build();

            try
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<CoHabitDbContext>();
                var logger = services.GetRequiredService<ILogger<Program>>();
                
                logger.LogInformation("Applying pending migrations...");
                context.Database.Migrate();
                logger.LogInformation("Migrations applied successfully!");
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database");
                
                // Trong production, bạn có thể muốn app dừng lại nếu migration fail
                if (app.Environment.IsProduction())
                {
                    throw;
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // CORS phải được đặt TRƯỚC Authentication/Authorization
            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            
            // Map SignalR hub với CORS
            app.MapHub<ChatHub>("/chathub", options =>
            {
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets | 
                                        Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
            })
            .RequireCors("AllowFrontend"); // Áp dụng CORS policy cho hub
            
            app.Run();
        }
    }
}
