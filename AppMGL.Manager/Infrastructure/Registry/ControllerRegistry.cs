using AppMGL.Manager.Infrastructure.Convention;

namespace AppMGL.Manager.Infrastructure.Registry
{
    public class ControllerRegistry : StructureMap.Registry
	{
		public ControllerRegistry()
		{
			Scan(scan =>
			{
				scan.TheCallingAssembly();
                scan.With(new ControllerConvention());
			});
		}
	}
}