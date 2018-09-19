using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using BCServerlessDemo.DataAndFunctions.Core.Service.Search;
using BCServerlessDemo.DataAndFunctions.Core.Service.Synchronisers;
using BCServerlessDemo.DataAndFunctions.Functions.Api;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleInjector;
using System;

namespace BCServerlessDemo.DataAndFunctions.Functions.Startup
{
    public static class ContainerRegistrar
    {
        public static Lazy<Container> _container = new Lazy<Container>(() =>
        {
            var container = new Container();
            RegisterTypes(container);
            container.Verify();
            return container;
        });


        public static Container GetContainer()
        {
            return _container.Value;   
        }      

        private static void RegisterTypes(Container container)
        {
            container.Register<IApplicationConfig, ApplicationConfig>();             

            container.Register<IDocumentClient>(() =>
            {
                var appConfig = container.GetInstance<IApplicationConfig>();
                return new DocumentClient(new Uri(appConfig.Endpoint), appConfig.AuthKey, connectionPolicy: new ConnectionPolicy { EnableEndpointDiscovery = false },
                    serializerSettings: new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            },Lifestyle.Singleton);

            //Repos
            container.Register<IDocumentDBRepository<ChangeRecord>, DocumentDBRepository<ChangeRecord>>();
            container.Register<IDocumentDBRepository<Config>, DocumentDBRepository<Config>>();
            container.Register<IDocumentDBRepository<Journey>, DocumentDBRepository<Journey>>();
            container.Register<IDocumentDBRepository<EventReminder>, DocumentDBRepository<EventReminder>>();
            container.Register<IDocumentDBRepository<SavedFile>, DocumentDBRepository<SavedFile>>();
            container.Register<IDocumentDBRepository<Metadata>, DocumentDBRepository<Metadata>>();
            container.Register<IDocumentDBRepository<Organisation>, DocumentDBRepository<Organisation>>();       
            container.Register<IDocumentDBRepository<Driver>, DocumentDBRepository<Driver>>();
            container.Register<IDocumentDBRepository<Vehicle>, DocumentDBRepository<Vehicle>>();
            container.Register<IUserDBRepository, UserDBRepository>();   

            //Services
            container.Register<IFileService, FileService>();
            container.Register<IJourneyService, JourneyService>();
            container.Register<ISearchService, SearchService>();
            container.Register<IDriverService, DriverService>();
            container.Register<IDriverSynchroniser, DriverSynchroniser>();
            container.Register<IOrganisationSynchroniser, OrganisationSynchroniser>();
            container.Register<IQueryBuilder<ChangeRecord, ChangeRecord>, ChangeRecordQueryBuilder>();
            container.Register<IQueryBuilder<Journey, Journey>, JourneyQueryBuilder>();
            container.Register<IQueryBuilder<Organisation, Organisation>, OrganisationQueryBuilder>();
            container.Register<IQueryBuilder<Driver, Driver>, DriverQueryBuilder>();
            container.Register<IUserService, UserService>();
            container.Register<IUserDigestService, UserDigestService>();
            container.Register<IOrganisationService, OrganisationService>();
            container.Register<IVehicleService, VehicleService>();

            //Apis
            container.Register<IJourneysApi, JourneysApi>();
            container.Register<IChangeRecordsApi, ChangeRecordsApi>(); 
            container.Register<IFileApi, FileApi>();
            container.Register<ISampleDataApi, SampleDataApi>();
            container.Register<IStartupApi, StartupApi>();
            container.Register<IMetadataApi, MetadataApi>();
            container.Register<IOrganisationLogoApi, OrganisationLogoApi>();
            container.Register<IOrganisationsApi, OrganisationsApi>();
            container.Register<IAdminDashboardOverviewApi, AdminDashboardOverviewApi>();
            container.Register<IDriversApi, DriversApi>();
            container.Register<IDriverPhotoApi, DriverPhotoApi>();
            container.Register<ISearchApi, SearchApi>();
            container.Register<IUsersApi, UsersApi>();
            container.Register<IVehiclesApi, VehiclesApi>();
        }
    }
}
