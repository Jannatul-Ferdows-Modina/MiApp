using AppMGL.Manager.Infrastructure.Tasks;

namespace AppMGL.Manager.Infrastructure.Registry
{
    public class TaskRegistry : StructureMap.Registry
	{
		public TaskRegistry()
		{
			Scan(scan =>
			{
				scan.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("AppMGL"));
				scan.AddAllTypesOf<IRunAtInit>();
				scan.AddAllTypesOf<IRunAtStartup>();
				scan.AddAllTypesOf<IRunOnEachRequest>();
				scan.AddAllTypesOf<IRunOnError>();
				scan.AddAllTypesOf<IRunAfterEachRequest>();
			});
		}
	}
}