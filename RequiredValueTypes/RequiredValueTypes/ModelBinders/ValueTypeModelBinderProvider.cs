using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RequiredValueTypes.RequiredValueTypes.ModelBinders;

public class ValueTypeModelBinderProvider : IModelBinderProvider {
    private IList<IModelBinderProvider> _providers { get; }

    public ValueTypeModelBinderProvider(IList<IModelBinderProvider> providers) {
        _providers = providers;
    }

    public IModelBinder? GetBinder(ModelBinderProviderContext context) {
        if (context is null) {
            throw new ArgumentNullException(nameof(context));
        }

        var type = context.Metadata.ModelType;
        bool isValueType = type.IsValueType;
        bool isNullable = Nullable.GetUnderlyingType(type) != null;

        if (isValueType && !isNullable) {
            var defaultBinder = _providers.Where(x => x.GetType() != GetType())
                                          .Select(x => x.GetBinder(context))
                                          .FirstOrDefault(x => x is not null);

            if (defaultBinder is null) {
                throw new InvalidOperationException("Value type can't be bound.");
            }

            return new ValueTypeModelBinder(defaultBinder);
        }

        return null;
    }
}
