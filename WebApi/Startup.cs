using System.Net;
using System.Web.Http;
using System.Web.Http.Tracing;
using Owin;

namespace WebApiHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                    "DefaultApi",
                    "api/{controller}/{id}",
                    new { id = RouteParameter.Optional }
                );

            // Load basic support for sending WebHooks
            config.InitializeCustomWebHooks();

            // Use SQL for persisting subscriptions
            config.InitializeCustomWebHooksSqlStorage();

            // Load Web API controllers for managing subscriptions
            config.InitializeCustomWebHooksApis();
            
            var listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;

            var traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Error;

            appBuilder.UseWebApi(config);
        }
    }
}