using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RequiredValueTypes.RequiredValueTypes.ModelBinders;

public class ValueTypeModelBinder : IModelBinder
{
    private readonly IModelBinder _defaultBinder;

    public ValueTypeModelBinder(IModelBinder defaultBinder)
    {
        _defaultBinder = defaultBinder;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var field = bindingContext.ModelName; // e.g. a property called "Id"
        var value = bindingContext.ValueProvider.GetValue(field);

        if (value == ValueProviderResult.None)
        {
            var error = $"The {field} field is required!";
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
