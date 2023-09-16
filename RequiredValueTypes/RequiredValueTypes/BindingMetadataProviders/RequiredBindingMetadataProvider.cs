using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace RequiredValueTypes.RequiredValueTypes.BindingMetadataProviders;

public class RequiredBindingMetadataProvider : IBindingMetadataProvider
{
    public void CreateBindingMetadata(BindingMetadataProviderContext context)
    {
        if (context.PropertyAttributes?.OfType<RequiredAttribute>().Any() is true)
        {
            context.BindingMetadata.IsBindingRequired = true;
        }
    }
}
