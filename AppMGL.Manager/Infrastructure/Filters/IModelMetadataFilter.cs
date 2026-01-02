using System;
using System.Collections.Generic;

namespace AppMGL.Manager.Infrastructure.Filters
{
	public interface IModelMetadataFilter
	{
		void TransformMetadata(System.Web.Mvc.ModelMetadata metadata,
			IEnumerable<Attribute> attributes);
	}
}