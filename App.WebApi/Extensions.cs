using App.Application.Interfaces.Core;
using App.Infrastructure.Repository.Core;
using App.WebApi.Hubs;
using App.WebApi.Infrastructure;
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer((obj) =>
            {
                obj.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                    {
                        var _jwtClass = new JwtClass(config);
                        var _configJwt = _jwtClass.LeerConfig();
                        validationParameters.ValidIssuer = _configJwt.Issuer;
                        validationParameters.ValidAudience = _configJwt.Audience;
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configJwt.SecretKey));
                        return new List<SecurityKey>() { key };
                    },
                    //Validar la reclamación del emisor JWT (iss)
                    ValidateIssuer = true,
                    //Validar la reclamación de audiencia JWT (aud)
                    ValidateAudience = true,
                    //Validar la caducidad del token
                    ValidateLifetime = true,
                    //Si desea permitir una cierta cantidad de desviación del reloj, configúrelo aquí
                    ClockSkew = TimeSpan.Zero,
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


                    OnTokenValidated = context =>
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

                        var _usuarioClass = new UsuarioClass(config);

                        if (!Guid.TryParse(_stampSecurity, out var stampSecurityGuid))
                            throw new Exception($"{JwtClass.ClaimSecurity} no es un UUID válido");

                        if (!Guid.TryParse(_usuarioId, out var usuarioGuid))
                            throw new Exception($"{JwtClass.ClaimUsuarioId} no es un UUID válido");

                        var _usuario = _usuarioClass.ConsultarPorId(usuarioGuid);

                        if (_usuario == null)
                        {
                            context.Fail("Usuario no encontrado");
                        }
                        else
                        {
                            if (_usuario.sello_seguridad != stampSecurityGuid || !_usuario.activo)
                            {
                                context.Fail("No Autorizado");
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IAliveCheckEstadoRepository, AliveCheckEstadoRepository>();
            services.AddTransient<IAliveCheckRepository, AliveCheckRepository>();
            services.AddTransient<IAliveCheckRespuestaRepository, AliveCheckRespuestaRepository>();

            services.AddTransient<IAsignacionEventoRepository, AsignacionEventoRepository>();
            services.AddTransient<IAsignacionEventoTipoRepository, AsignacionEventoTipoRepository>();
            services.AddTransient<IAsignacionRepository, AsignacionRepository>();

            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IClienteUsuarioRepository, ClienteUsuarioRepository>();

            services.AddTransient<IConfiguracionRepository, ConfiguracionRepository>();

            services.AddTransient<IControlPointRepository, ControlPointRepository>();

            services.AddTransient<IIncidenciaAdjuntoRepository, IncidenciaAdjuntoRepository>();
            services.AddTransient<IIncidenciaRepository, IncidenciaRepository>();
            services.AddTransient<IIncidenteTipoRepository, IncidenteTipoRepository>();

            services.AddTransient<IOcurrenciaAdjuntoRepository, OcurrenciaAdjuntoRepository>();
            services.AddTransient<IOcurrenciaRepository, OcurrenciaRepository>();
            services.AddTransient<IOcurrenciaTipoRepository, OcurrenciaTipoRepository>();

            services.AddTransient<IPanicAlertAdjuntoRepository, PanicAlertAdjuntoRepository>();
            services.AddTransient<IPanicAlertAdjuntoTipoRepository, PanicAlertAdjuntoTipoRepository>();
            services.AddTransient<IPanicAlertEstadoRepository, PanicAlertEstadoRepository>();
            services.AddTransient<IPanicAlertRecepcionRepository, PanicAlertRecepcionRepository>();
            services.AddTransient<IPanicAlertRepository, PanicAlertRepository>();

            services.AddTransient<IPuestoRepository, PuestoRepository>();
            services.AddTransient<IRolRepository, RolRepository>();
            services.AddTransient<IRondaAdjuntoRepository, RondaAdjuntoRepository>();
            services.AddTransient<IRondaRepository, RondaRepository>();

            services.AddTransient<ISesionUsuarioEvidenciaRepository, SesionUsuarioEvidenciaRepository>();
            services.AddTransient<ISesionUsuarioRepository, SesionUsuarioRepository>();

            services.AddTransient<ITipoConfiguracionRepository, TipoConfiguracionRepository>();
            services.AddTransient<IUnidadRepository, UnidadRepository>();

            services.AddTransient<IUsuarioEstadoRepository, UsuarioEstadoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IUsuarioRolRepository, UsuarioRolRepository>();
            services.AddTransient<IUsuarioUnidadRepository, UsuarioUnidadRepository>();
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

                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Please insert JWT with Bearer into field",
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey
                //});

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

        public static string? GetConnectionString(this IConfiguration configuration)
        {
            return configuration?.GetSection("ConnectionStrings")["GuardiasDb"];
        }
    }
}
