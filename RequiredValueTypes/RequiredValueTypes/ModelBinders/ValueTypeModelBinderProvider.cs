using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RequiredValueTypes.RequiredValueTypes.ModelBinders;

public class ValueTypeModelBinderProvider : IModelBinderProvider {
    private readonly IList<IModelBinderProvider> _providers;
    private readonly bool _isRequiredImplicit;

    public ValueTypeModelBinderProvider(IList<IModelBinderProvider> providers, bool isRequiredImplicit) {
        _providers = providers;
        _isRequiredImplicit = isRequiredImplicit;
    }

    public IModelBinder? GetBinder(ModelBinderProviderContext context) {
        if (context is null) {
            throw new ArgumentNullException(nameof(context));
        }

        var type = context.Metadata.ModelType;

        bool isValueType = type.IsValueType;
        bool isNullable = Nullable.GetUnderlyingType(type) is not null;

        if (isValueType && !isNullable) {
            var defaultBinder = _providers.Where(x => x.GetType() != GetType())
                                          .Select(x => x.GetBinder(context))
                                          .FirstOrDefault(x => x is not null);

            if (defaultBinder is null) {
                throw new InvalidOperationException("Value type can't be bound.");
            }

            return new ValueTypeModelBinder(defaultBinder, _isRequiredImplicit);
        }

        return null;
    }
}
