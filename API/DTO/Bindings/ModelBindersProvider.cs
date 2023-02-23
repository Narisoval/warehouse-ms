using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Warehouse.API.DTO.Bindings;

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
            
            default:
                return null;
        }
    }
}