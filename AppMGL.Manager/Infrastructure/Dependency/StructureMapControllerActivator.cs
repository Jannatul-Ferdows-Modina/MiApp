using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace AppMGL.Manager.Infrastructure.Dependency
{
    public class StructureMapControllerActivator : IHttpControllerActivator
    {
        public StructureMapControllerActivator(HttpConfiguration configuration) { }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = (new Container()).GetInstance(controllerType) as IHttpController;
            return controller;
        }
    }
}