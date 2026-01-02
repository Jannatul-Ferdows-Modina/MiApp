using System;
using System.Web;
using System.Web.Mvc;
using AppMGL.Manager.Infrastructure.Dependency;
using AppMGL.Manager.Infrastructure.Registry;
using AppMGL.Manager.Infrastructure.Tasks;
using StructureMap;
using System.Web.Routing;

namespace AppMGL.Manager
{
    public class WebApiApplication : HttpApplication
    {
        public IContainer _Container
        {
            get
            {
                return (IContainer)HttpContext.Current.Items["_Container"];
            }
            set
            {
                HttpContext.Current.Items["_Container"] = value;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
           // App_Start.RouteConfig.RegisterRoutes(RouteTable.Routes);
            _Container = new Container();
            _Container.Configure(cfg =>
            {
                cfg.AddRegistry(new StandardRegistry());
                cfg.AddRegistry(new ControllerRegistry());
                cfg.AddRegistry(new ActionFilterRegistry(() => _Container));
                cfg.AddRegistry(new WebApiRegistry());
                cfg.AddRegistry(new TaskRegistry());
                cfg.AddRegistry(new ModelMetadataRegistry());
            });

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(() => _Container));

            using (var container = _Container.GetNestedContainer())
            {
                foreach (var task in container.GetAllInstances<IRunAtInit>())
                {
                    task.Execute();
                }

                foreach (var task in container.GetAllInstances<IRunAtStartup>())
                {
                    task.Execute();
                }
            }
        }

        public void Application_BeginRequest()
        {
            if (_Container == null) return;

            _Container = _Container.GetNestedContainer();

            foreach (var task in _Container.GetAllInstances<IRunOnEachRequest>())
            {
                task.Execute();
            }
        }

        public void Application_Error()
        {
            if (_Container == null) return;

            foreach (var task in _Container.GetAllInstances<IRunOnError>())
            {
                task.Execute();
            }
        }

        public void Application_EndRequest()
        {
            if (_Container == null) return;

            try
            {
                foreach (var task in _Container.GetAllInstances<IRunAfterEachRequest>())
                {
                    task.Execute();
                }
            }
            finally
            {
                _Container.Dispose();
                _Container = null;
            }
        }
    }
}