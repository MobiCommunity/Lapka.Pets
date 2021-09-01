using System;
using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.CQRS.Queries;
using Convey.HTTP;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.WebApi;
using Convey.WebApi.Exceptions;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Exceptions;
using Lapka.Pets.Infrastructure.Options;
using Lapka.Pets.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lapka.Pets.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                // .AddRabbitMq()
                .AddJwt()
                .AddMongo()
                .AddMongoRepository<PetShelterDocument, Guid>("petsshelter")
                .AddMongoRepository<PetUserDocument, Guid>("petsuser")
                .AddMongoRepository<LostPetDocument, Guid>("lostpets")
                // .AddConsul()
                // .AddFabio()
                // .AddMessageOutbox()
                // .AddMetrics()
                ;
            
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            
            builder.Services.Configure<KestrelServerOptions>
                (o => o.AllowSynchronousIO = true);

            builder.Services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);

            IServiceCollection services = builder.Services;
            
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            FilesMicroserviceOptions filesMicroserviceOptions = new FilesMicroserviceOptions();
            configuration.GetSection("filesMicroservice").Bind(filesMicroserviceOptions);
            
            IdentityMicroserviceOptions identityMicroserviceOptions = new IdentityMicroserviceOptions();
            configuration.GetSection("identityMicroservice").Bind(identityMicroserviceOptions);

            services.AddSingleton(filesMicroserviceOptions);
            services.AddSingleton(identityMicroserviceOptions);

            services.AddGrpcClient<Photo.PhotoClient>(o =>
            {
                o.Address = new Uri(filesMicroserviceOptions.UrlHttp2);
            });
            
            services.AddGrpcClient<PetGrpc.PetGrpcClient>(o =>
            {
                o.Address = new Uri(identityMicroserviceOptions.UrlHttp2);
            });
            
            services.AddScoped<IGrpcPetService, GrpcPetService>();
            
            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();

            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, DummyMessageBroker>();

            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithTransientLifetime());

            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                .UseAuthentication()
                //.UseMetrics()
                //.UseRabbitMq()
                ;


            return app;
        }
        
        public static async Task<Guid> AuthenticateUsingJwtAsync(this HttpContext context)
        {
            var authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            return authentication.Succeeded ? Guid.Parse(authentication.Principal.Identity.Name) : Guid.Empty;
        }
    }
}