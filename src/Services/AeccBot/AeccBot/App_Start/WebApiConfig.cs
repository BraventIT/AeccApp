using AeccBot.CommandHandling;
using AeccBot.MessageRouting;
using AeccBot.MessageRouting.DataStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace AeccBot
{
    public static class WebApiConfig
    {
        public static MessageRouterManager MessageRouterManager
        {
            get;
            private set;
        }

        public static IRoutingDataManager RoutingDataManager
        {
            get;
            private set;
        }

        public static IMessageRouterResultHandler MessageRouterResultHandler
        {
            get;
            private set;
        }

        public static IBotCommandHandler CommandMessageHandler
        {
            get;
            private set;
        }

        public static IBotCommandHandler BackChannelMessageHandler
        {
            get;
            private set;
        }

        public static void Register(HttpConfiguration config)
        {
            // Json settings
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            // Web API configuration and services
           // config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Message routing
            MessageRouterManager = new MessageRouterManager();
            RoutingDataManager = new LocalRoutingDataManager();
            MessageRouterResultHandler = new MessageRouterResultHandler();
            CommandMessageHandler = new CommandMessageHandler();
            BackChannelMessageHandler = new BackChannelMessageHandler();
        }
    }
}
