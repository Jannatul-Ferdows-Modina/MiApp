using AutoMapper.Configuration;

namespace AppMGL.Manager.Infrastructure.Mapping
{
	public interface IHaveCustomMappings
	{
		void CreateMappings(IConfiguration configuration);
	}
}