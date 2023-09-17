using RequiredValueTypes.RequiredValueTypes.ModelBinders;
using RequiredValueTypes.RequiredValueTypes.ValueProviders;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RequiredValueTypes.RequiredValueTypes.BindingMetadataProviders;
using System.ComponentModel.DataAnnotations;

namespace RequiredValueTypes.RequiredValueTypes;

public static class RequiredValueTypesExtensions {
    /// <summary>
    /// Require all non-nullable value types in input models to be provided implicitly.
    /// Custom error message formats given to <see cref="RequiredAttribute"/> will be shown.
    /// 
    /// Value types cannot be given default values with this implementation, as all of the values must be
    /// provided by the user.
    /// </summary>
    public static IServiceCollection UseImplicitRequiredValueTypes(this IServiceCollection services) {
        services.Configure<MvcOptions>(options => {
            options.ValueProviderFactories.Insert(0, new JsonValueProviderFactory());
            options.ModelBinderProviders.RemoveType<BodyModelBinderProvider>();
            options.ModelBinderProviders.Insert(0, new ValueTypeModelBinderProvider(
                options.ModelBinderProviders,
                isRequiredImplicit: true));
        });

        return services;
    }

    /// <summary>
    /// Require all non-nullable value types in input models to be provided explicitly through <see cref="RequiredAttribute"/>.
    /// </summary>
    public static IServiceCollection UseExplicitRequiredValueTypes(this IServiceCollection services) {
        services.Configure<MvcOptions>(options => {
            options.ValueProviderFactories.Insert(0, new JsonValueProviderFactory());
            options.ModelBinderProviders.RemoveType<BodyModelBinderProvider>();
            options.ModelBinderProviders.Insert(0, new ValueTypeModelBinderProvider(
                options.ModelBinderProviders,
                isRequiredImplicit: false));
        });

        return services;
    }

    /// <summary>
    /// Require all value types with <see cref="RequiredAttribute"/> to be bound. This doesn't include input
    /// models provided through <see cref="FromBodyAttribute"/>, for example JSON. No support for custom
    /// error messages is included.
    /// </summary>
    public static IServiceCollection UseRequiredValueTypesExceptFromBody(this IServiceCollection services) {
        services.Configure<MvcOptions>(options => {
            options.ModelMetadataDetailsProviders.Add(new RequiredBindingMetadataProvider());
        });

        return services;
    }
}
