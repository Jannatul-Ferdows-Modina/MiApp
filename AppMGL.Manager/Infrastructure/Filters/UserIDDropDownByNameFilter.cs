using System;
using System.Collections.Generic;

namespace AppMGL.Manager.Infrastructure.Filters
{
	public class UserIdDropDownByNameFilter : IModelMetadataFilter
	{
		public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata,
			IEnumerable<Attribute> attributes)
		{
			if (!string.IsNullOrEmpty(metadata.PropertyName) &&
				string.IsNullOrEmpty(metadata.DataTypeName) &&
				metadata.PropertyName.ToLower().Contains("assignedto"))
			{
				metadata.DataTypeName = "UserId";
			}
		}
	}
}