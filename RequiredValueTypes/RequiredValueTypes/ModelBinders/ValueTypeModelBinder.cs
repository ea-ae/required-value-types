using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RequiredValueTypes.RequiredValueTypes.ModelBinders;

public class ValueTypeModelBinder : IModelBinder
{
    private readonly IModelBinder _defaultBinder;
    private readonly bool _isRequiredImplicit;

    public ValueTypeModelBinder(IModelBinder defaultBinder, bool isRequiredImplicit)
    {
        _defaultBinder = defaultBinder;
        _isRequiredImplicit = isRequiredImplicit;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var field = bindingContext.ModelName; // e.g. a property called "Id"
        var value = bindingContext.ValueProvider.GetValue(field);

        var metadata = bindingContext.ModelMetadata.ValidatorMetadata;
        var requiredAttribute = metadata.OfType<RequiredAttribute>().FirstOrDefault();

        bool isRequired = _isRequiredImplicit || requiredAttribute is not null;

        if (isRequired && value == ValueProviderResult.None)
        {
            var error = requiredAttribute?.ErrorMessage ?? $"The {field} field is required.";
            bindingContext.ModelState.TryAddModelError(field, error);
            return;
        }

        bindingContext.ModelState.SetModelValue(field, value);

        if (_defaultBinder is not null)
        {
            await _defaultBinder.BindModelAsync(bindingContext);
        }
    }
}
