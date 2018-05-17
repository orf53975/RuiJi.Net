﻿using Microsoft.Owin.Hosting;
using Microsoft.Owin.FileSystems;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin;
using Microsoft.Owin.Host.HttpListener;

namespace RuiJi.Owin
{
    public class Startup
    {
        static Startup()
        {
            OwinServerFactory.Initialize(new Dictionary<string, object>());
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage();

            var config = GetWebApiConfig();
            app.UseWebApi(config);

            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(@".\www")
            });

            app.UseWelcomePage("");
        }

        private HttpConfiguration GetWebApiConfig()
        {

            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "Request",
                routeTemplate: "api/request",
                defaults: new { controller = "CrawlerApi", action = "request" }
            );

            config.Routes.MapHttpRoute(
                name: "ServerInfo",
                routeTemplate: "api/crawler/info",
                defaults: new { controller = "CrawlerApi", action = "serverinfo" }
            );

            config.Routes.MapHttpRoute(
                name: "ProxyRequest",
                routeTemplate: "api/proxy/request",
                defaults: new { controller = "CrawlerProxyApi", action = "request" }
            );

            config.Routes.MapHttpRoute(
                name: "PingCP",
                routeTemplate: "api/cp/ping",
                defaults: new { controller = "CrawlerProxyApi", action = "Ping" }
            );

            config.Routes.MapHttpRoute(
                name: "PingEP",
                routeTemplate: "api/ep/ping",
                defaults: new { controller = "CrawlerProxyApi", action = "Ping" }
            );

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );


            //config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
            };

            return config;
        }
    }
}