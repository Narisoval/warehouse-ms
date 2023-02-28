using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Warehouse.API.Common.Bindings;

public class ModelBindersProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var modelType = context.Metadata.ModelType;

        switch (modelType)
        {
            case var _ when modelType == typeof(Provider):
                return new BinderTypeModelBinder(typeof(ProviderEntityModelBinder));
            
            case var _ when modelType == typeof(Brand):
                return new BinderTypeModelBinder(typeof(BrandEntityModelBinder));
            
            case var _ when modelType == typeof(Category):
                return new BinderTypeModelBinder(typeof(CategoryEntityModelBinder));
            default:
                return null;
        }
    }
}