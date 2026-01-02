namespace AppMGL.Manager.Infrastructure.Registry
{
    public class StandardRegistry : StructureMap.Registry
	{
		public StandardRegistry()
		{
			Scan(scan =>
			{
				scan.TheCallingAssembly();
                scan.WithDefaultConventions();
			});
		}
	}
}