using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using sapo_clone.API.Settings;
using sapo_clone.Domain.Entities;
using sapo_clone.Infrastructure.Data;
using sapo_clone.Infrastructure.Data.Interfaces;

namespace sapo_clone.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection(nameof(CorsSetting)).Get<CorsSetting>();
            ArgumentNullException.ThrowIfNull(corsSettings, nameof(CorsSetting));
            services.AddCors(options => {
                foreach (var policy in corsSettings.Policies)
                {
                    options.AddPolicy(
                        policy.Name, builder => {
                            builder.WithOrigins(policy.AllowedOrigins)
                                .WithMethods(policy.AllowedMethods)
                                .WithHeaders(policy.AllowedHeaders);
                            if (policy.AllowCredentials)
                                builder.AllowCredentials();
                        }
                    );
                }
            });
            return services;
        }
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "TCT DATA API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            return services;
        }
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                // options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<SapoDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IApplicationBuilder EnsureMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Migrate();

            return app;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    options.DefaultChallengeScheme =
                        options.DefaultForbidScheme =
                            options.DefaultScheme =
                                options.DefaultSignInScheme =
                                    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]!)),
                    ClockSkew = new TimeSpan(0, 0, 5)
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
