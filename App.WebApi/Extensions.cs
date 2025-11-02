using App.Application.Interfaces;
using App.WebApi.Hubs;
using App.WebApi.Models.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace App.WebApi
{
    public static class Extensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureAuthJwt(this IServiceCollection services, IConfiguration config)
        {
            var _jwtClass = new JwtClass(config);
            var _configJwt = _jwtClass.LeerConfig();
            var _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configJwt.SecretKey));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer((obj) =>
            {
                obj.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _signingKey,
                    ValidIssuer = _configJwt.Issuer,
                    ValidAudience = _configJwt.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                obj.UseSecurityTokenValidators = true;

                obj.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        // Solo aplica para conexiones WebSocket como SignalR
                        var accessToken = context.Request.Query["access_token"];

                        // Aquí verifica si la solicitud es al Hub
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/tracker"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },


                    OnTokenValidated = async context =>
                    {
                        //Debe existir el claim usuarioId
                        if (!context.Principal!.HasClaim(c => c.Type == JwtClass.ClaimUsuarioId))
                        {
                            context.Fail($"No se encontró '{JwtClass.ClaimUsuarioId}' en la sesión");
                        }

                        //Debe existir el ClaimSecurity
                        if (!context.Principal.HasClaim(c => c.Type == JwtClass.ClaimSecurity))
                        {
                            context.Fail($"No se encontró '{JwtClass.ClaimSecurity}' en la sesión");
                        }

                        //Valida que el sello de seguridad sea el mismo del usuario
                        var _usuarioId = context.Principal.Claims.First(c => c.Type == JwtClass.ClaimUsuarioId).Value;
                        var _stampSecurity = context.Principal.Claims.First(c => c.Type == JwtClass.ClaimSecurity).Value;

                        if (!Guid.TryParse(_stampSecurity, out var stampSecurityGuid))
                            throw new Exception($"{JwtClass.ClaimSecurity} no es un UUID válido");

                        if (!Guid.TryParse(_usuarioId, out var usuarioGuid))
                            throw new Exception($"{JwtClass.ClaimUsuarioId} no es un UUID válido");

                        // --- RESOLVER IUnitOfWork DESDE DI ---
                        var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
                        var _usuario = await unitOfWork.Usuarios.GetByIdAsync(usuarioGuid);

                        if (_usuario == null)
                        {
                            context.Fail("Usuario no encontrado");
                            return;
                        }

                        if (_usuario.sello_seguridad != stampSecurityGuid || _usuario.deleted_at != null)
                        {
                            context.Fail("No Autorizado");
                        }
                    }
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var commentsFileNameApi = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            var commentsFileApi = Path.Combine(baseDirectory, commentsFileNameApi);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Guardias Api",
                    Version = "v1"
                });

                if (File.Exists(commentsFileApi))
                {
                    c.IncludeXmlComments(commentsFileApi);
                }

                c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field"
                });

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
                        new string[] { }
                    }
                });
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var error = context.Features.Get<IExceptionHandlerPathFeature>();

                if (error?.Error is ApiException)
                {
                    var ex = error.Error as ApiException;
                    await context.Response.WriteAsync(ex?.ToString() ?? "Error desconocido");
                }
                else
                {
                    await context.Response.WriteAsync(new ApiException(error?.Error?.Message ?? "Error desconocido").ToString());
                }
            }));
        }

        public static void ConfigureHubs(this IEndpointRouteBuilder app)
        {
            app.MapHub<TrackerHub>("/hubs/tracker");
        }

        public static Guid Id(this ClaimsPrincipal user)
        {
            var _usuarioId = user?.Claims?.FirstOrDefault(c => c.Type.Equals(JwtClass.ClaimUsuarioId, StringComparison.OrdinalIgnoreCase))?.Value;

            if (_usuarioId != null)
            {
                if (Guid.TryParse(_usuarioId, out var guid))
                    return guid;
            }

            throw new Exception("No se encontró el ID de usuario en la sesión");
        }
    }
}
