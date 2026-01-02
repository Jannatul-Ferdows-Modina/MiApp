using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AppMGL.Manager.Infrastructure.Filters;
using WebGrease.Css.Extensions;

namespace AppMGL.Manager.Infrastructure.Provider
{
	public class ExtensibleModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		private readonly IModelMetadataFilter[] _metadataFilters;

		public ExtensibleModelMetadataProvider(IModelMetadataFilter[] metadataFilters)
		{
			_metadataFilters = metadataFilters;
		}

		protected override ModelMetadata CreateMetadata(
			IEnumerable<Attribute> attributes,
			Type containerType,
			Func<object> modelAccessor,
			Type modelType,
			string propertyName)
		{
			var metadata = base.CreateMetadata(
				attributes,
				containerType,
				modelAccessor,
				modelType,
				propertyName);

			_metadataFilters.ForEach(m => m.TransformMetadata(metadata, attributes));

			return metadata;
		}
	}
}