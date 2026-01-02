using System.Web.Mvc;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;
using StructureMap.Pipeline;

namespace AppMGL.Manager.Infrastructure.Convention
{
	public class ControllerConvention : IRegistrationConvention
	{
		public void ScanTypes(TypeSet types, StructureMap.Registry registry)
        {
            foreach (var type in types.AllTypes())
            {
                if (type.CanBeCastTo<Controller>() && !type.IsAbstract)
                {
                    registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
                }
            }

            //types.FindTypes(TypeClassification.All).Each(type =>
            //{
            //    if (type.CanBeCastTo(typeof(Controller)) && !type.IsAbstract)
            //    {
            //        type.GetInterfaces().Each(@interface => registry.For(@interface).Use(type));
            //    }
            //});
        }
    }
}