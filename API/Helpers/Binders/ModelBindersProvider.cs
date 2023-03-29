using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Warehouse.API.Helpers.Binders.EntityModelBinders;
using Warehouse.API.Helpers.Binders.FileModelBinders;

namespace Warehouse.API.Helpers.Binders;

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
            
            case var _ when modelType == typeof(Product):
                return new BinderTypeModelBinder(typeof(ProductEntityModelBinder));
            
            case var _ when modelType == typeof(List<IFormFile>):
                return new BinderTypeModelBinder(typeof(ImageFileModelBinder));
            
            default:
                return null;
        }
    }
}