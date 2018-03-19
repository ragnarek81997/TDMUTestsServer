using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Swashbuckle.Application;
using System;
using System.Configuration;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net;

[assembly: OwinStartupAttribute(typeof(TDMUTestsServer.Web.Startup))]
namespace TDMUTestsServer.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
            /*
            // Register new UserProvider
            var idProvider = new UserProvider();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            // Make long polling connections wait a maximum of 110 seconds for a
            // response. When that time expires, trigger a timeout command and
            // make the client reconnect.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

            GlobalHost.Configuration.TransportConnectTimeout = TimeSpan.FromSeconds(20);

            // Wait a maximum of 30 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);

            app.MapSignalR();
            */
        }
    }
}
