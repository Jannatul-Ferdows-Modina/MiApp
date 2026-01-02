using System.Web.Mvc;

namespace AppMGL.Manager.Areas.Operation
{
    public class HomenAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Home";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Home_default",
                "Home/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}