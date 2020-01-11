using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AirlineManagementWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
             name: "SearchApi",
             routeTemplate: "api/{controller}/{action}/{id}",
             defaults: new { id = RouteParameter.Optional }
            );
            //config.Routes.MapHttpRoute(
            // name: "SearchApi4p",
            // routeTemplate: "api/{controller}/{action}/{typeName}/{flightId}/{coutry}/{company}",
            // defaults: new { typeName = RouteParameter.Optional, flightId = RouteParameter.Optional , coutry = RouteParameter.Optional, company = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute("SearchApi", "api/{controller}/{action}/{id}",
            //        new
            //        {
            //            id = RouteParameter.Optional
            //        });
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(
                new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }
    }
}
