using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.CQRS.Queries;
using Convey.HTTP;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.WebApi;
using Convey.WebApi.Exceptions;
using Lapka.Pets.Application.Commands.LostPets;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Commands.UserPets;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Events.External;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Elastic.Services;
using Lapka.Pets.Infrastructure.Exceptions;
using Lapka.Pets.Infrastructure.Grpc.Services;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Lapka.Pets.Infrastructure.Mongo.Repositories;
using Lapka.Pets.Infrastructure.Options;
using Lapka.Pets.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

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
                .AddRabbitMq()
                .AddJwt()
                .AddMongo()
                .AddMongoRepository<ShelterDocument, Guid>("shelters")
                .AddMongoRepository<ShelterPetDocument, Guid>("petsshelter")
                .AddMongoRepository<UserPetDocument, Guid>("petsuser")
                .AddMongoRepository<LostPetDocument, Guid>("lostpets")
                .AddMongoRepository<LikePetDocument, Guid>("likedpets")
                .AddMessageOutbox()
                // .AddConsul()
                // .AddFabio()
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
            services.AddSingleton(filesMicroserviceOptions);

            IdentityMicroserviceOptions identityMicroserviceOptions = new IdentityMicroserviceOptions();
            configuration.GetSection("identityMicroservice").Bind(identityMicroserviceOptions);
            services.AddSingleton(identityMicroserviceOptions);

            ElasticSearchOptions elasticSearchOptions = new ElasticSearchOptions();
            configuration.GetSection("elasticSearch").Bind(elasticSearchOptions);
            services.AddSingleton(elasticSearchOptions);
            ConnectionSettings elasticConnectionSettings = new ConnectionSettings(new Uri(elasticSearchOptions.Url));

            services.AddGrpcClient<PhotoProto.PhotoProtoClient>(o =>
            {
                o.Address = new Uri(filesMicroserviceOptions.UrlHttp2);
            });

            services.AddGrpcClient<ShelterProto.ShelterProtoClient>(o =>
            {
                o.Address = new Uri(identityMicroserviceOptions.UrlHttp2);
            });

            services.AddHostedService<ElasticSearchSeeder>();

            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();
            services.AddSingleton<IElasticClient>(new ElasticClient(elasticConnectionSettings));

            services.AddScoped<IGrpcPhotoService, GrpcPhotoService>();

            services.AddTransient<IShelterRepository, ShelterRepository>();
            services.AddTransient<IPetLikeElasticsearchUpdater, PetLikeElasticsearchUpdater>();
            services.AddTransient<ILostPetElasticsearchUpdater, LostPetElasticsearchUpdater>();
            services.AddTransient<IShelterPetElasticsearchUpdater, ShelterPetElasticsearchUpdater>();
            services.AddTransient<IUserPetElasticsearchUpdater, UserPetElasticsearchUpdater>();
            services.AddTransient<IShelterPetRepository, ShelterPetRepository>();
            services.AddTransient<IUserPetRepository, UserPetRepository>();
            services.AddTransient<ILostPetRepository, LostPetRepository>();
            services.AddTransient<IPetLikeRepository, PetLikeRepository>();
            services.AddTransient<IGrpcIdentityService, GrpcIdentityService>();
            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, MessageBroker>();


            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithTransientLifetime());

            builder.Build();
            
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeCommand<DeleteShelterPet>()
                .SubscribeCommand<DeleteLostPet>()
                .SubscribeCommand<DeleteUserPet>()
                .SubscribeCommand<DeleteUserPetPhoto>()
                .SubscribeCommand<DeleteShelterPetPhoto>()
                .SubscribeCommand<DeleteLostPetPhoto>()
                .SubscribeEvent<ShelterAdded>()
                .SubscribeEvent<ShelterRemoved>()
                .SubscribeEvent<ShelterChanged>()
                .SubscribeEvent<ShelterOwnerAssigned>()
                .SubscribeEvent<ShelterOwnerUnassigned>()
                //.UseMetrics()
                ;


            return app;
        }

        public static async Task<Guid> AuthenticateUsingJwtGetUserIdAsync(this HttpContext context)
        {
            AuthenticateResult authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            return authentication.Succeeded ? Guid.Parse(authentication.Principal.Identity.Name) : Guid.Empty;
        }
        
    }
}