using System.Web.Mvc;
using AppMGL.Manager.Infrastructure.Filters;
using AppMGL.Manager.Infrastructure.Provider;

namespace AppMGL.Manager.Infrastructure.Registry
{
    public class ModelMetadataRegistry : StructureMap.Registry
	{
		public ModelMetadataRegistry()
		{
			For<ModelMetadataProvider>().Use<ExtensibleModelMetadataProvider>();

			Scan(scan =>
			{
				scan.TheCallingAssembly();
				scan.AddAllTypesOf<IModelMetadataFilter>();
			});
		}
	}
}