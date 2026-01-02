using System;
using System.Web.Mvc;
using AppMGL.Manager.Infrastructure.Provider;
using StructureMap;
using StructureMap.TypeRules;

namespace AppMGL.Manager.Infrastructure.Registry
{
	public class ActionFilterRegistry : StructureMap.Registry
	{
		public ActionFilterRegistry(Func<IContainer> containerFactory)
		{
			For<IFilterProvider>().Use(
				new StructureMapFilterProvider(containerFactory));

			Policies.SetAllProperties(x =>
				x.Matching(p =>
					p.DeclaringType.CanBeCastTo(typeof(ActionFilterAttribute)) &&
					p.DeclaringType.Namespace.StartsWith("AppMGL") &&
					!p.PropertyType.IsPrimitive &&
					p.PropertyType != typeof(string)));
		}
	}
}