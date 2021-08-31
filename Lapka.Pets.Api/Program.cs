using Convey;
using Convey.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Open.Serialization.Json.Newtonsoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Lapka.Pets.Api.Attributes;
using Lapka.Pets.Application;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure;
using Lapka.Pets.Infrastructure.Services;

namespace Lapka.Pets.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args).Build().RunAsync();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureServices(services =>
                {
                    services.AddControllers();

                    services.TryAddSingleton(new JsonSerializerFactory().GetSerializer());

                    services
                        .AddConvey()
                        .AddInfrastructure()
                        .AddApplication();

                    services.AddTransient<IPetRepository<ShelterPet>, ShelterPetRepository>();
                    services.AddTransient<IPetRepository<UserPet>, UserPetRepository>();
                    services.AddTransient<IPetRepository<LostPet>, LostPetRepository>();
                    services.AddScoped<IGrpcPhotoService, GrpcPhotoService>();
                    
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                    
                    services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "Pets Microservice",
                            Description = ""
                        });
                        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        string xmlFile2 = "Lapka.Pets.Application.xml";
                        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        string xmlPath2 = Path.Combine(AppContext.BaseDirectory, xmlFile2);
                        c.IncludeXmlComments(xmlPath);
                        c.IncludeXmlComments(xmlPath2);
                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = @"JWT Authorization header using the Bearer scheme.
                                           Enter 'Bearer' [space] and then your token in the text input below.
                                           Example: 'Bearer 12345abcdef'",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer"
                        });
                        
                        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                                },
                                new List<string>()
                            }
                        });
                    });

                    services.BuildServiceProvider();
                }).Configure(app =>
                {
                    app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                    
                    app
                        .UseConvey()
                        .UseInfrastructure()
                        .UseRouting()
                        .UseSwagger(c => { c.RouteTemplate = "api/pets/swagger/{documentname}/swagger.json"; })
                        .UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/api/pets/swagger/v1/swagger.json", "My API V1");
                            c.RoutePrefix = "api/pets/swagger";
                        })
                        .UseEndpoints(e =>
                        {
                            e.MapControllers();
                            e.Map("ping", async ctx => { await ctx.Response.WriteAsync("Alive"); });
                        });
                })
                .UseLogging();
    }
}