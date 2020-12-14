using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MasGlobal.AR.Employees.WebApiCore
{
    public static class ReflectionHelper
    {
        public static Object GetPropValue(this Object obj, String propName)
        {
            string[] nameParts = propName.Split('.');
            if (nameParts.Length == 1)
            {
                return obj.GetType().GetProperty(propName).GetValue(obj, null);
            }

            foreach (String part in nameParts)
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
    }

    public static class Helpers
    {
        public static Attribute GetMetadataAttribute(this IHtmlHelper html, Type baseClass, string property, Type attribute)
        {
            var metadataAttribute = baseClass.GetCustomAttribute(typeof(ModelMetadataTypeAttribute)) as ModelMetadataTypeAttribute;
            if (metadataAttribute == null)
                return null;

            var metadataType = metadataAttribute.MetadataType;
            var attrs = metadataType.GetProperty(property)?.GetCustomAttributes(attribute, false);
            if (attrs != null && attrs.Length > 0)
            {
                return (Attribute)attrs[0];
            }

            return null;
        }

        public static object GetModelProperty(this IHtmlHelper html, object o, string property)
        {
            var ret = o.GetType().GetProperty(property)?.GetValue(o);
            return ret;
        }
        
        public static object GetValueFromAnonymousType( object dataitem, string itemkey ) 
        {
            object itemvalue = null;
            if (dataitem != null)
            { 
                System.Type type = dataitem.GetType();
                itemvalue = type.GetProperty(itemkey).GetValue(dataitem, null);
            }
            return itemvalue;
            
        }
        
        public class TimeSpanModelBinderProvider : IModelBinderProvider
        {
            public IModelBinder GetBinder(ModelBinderProviderContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                if (context.Metadata.ModelType == typeof(TimeSpan))
                {
                    return new BinderTypeModelBinder(typeof(TimeSpanModelBinder));
                }
            
                return null;
            }
        }
        public class TimeSpanModelBinder : IModelBinder
        {
      
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }

                // Specify a default argument name if none is set by ModelBinderAttribute
                var modelName = bindingContext.BinderModelName;
                var valueProviderResult =
                    bindingContext.ValueProvider.GetValue(modelName);

                if (valueProviderResult == ValueProviderResult.None)
                {
                    return Task.CompletedTask;
                }

                bindingContext.ModelState.SetModelValue(modelName,
                    valueProviderResult);

                var value = valueProviderResult.FirstValue;
                // Check if the argument value is null or empty
                if (string.IsNullOrEmpty(value))
                {
                    return Task.CompletedTask;
                }

                if (!int.TryParse(value, out var totalMinutes))
                {
                    bindingContext.ModelState.TryAddModelError(
                        bindingContext.ModelName,
                        "Formato invalido");
                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(TimeSpan.FromMinutes(totalMinutes));
                return Task.CompletedTask;
            }
        }

    }
}