using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class DisplayEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var enumNames = Enum.GetValues(context.Type)
                .Cast<Enum>()
                .Select(value =>
                {
                    var displayAttr = value.GetType()
                        .GetMember(value.ToString())
                        .First()
                        .GetCustomAttributes(false)
                        .OfType<DisplayAttribute>()
                        .FirstOrDefault();
                    return displayAttr?.Name ?? value.ToString();
                })
                .ToList();

            schema.Enum.Clear();
            foreach (var name in enumNames)
            {
                schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(name));
            }

            schema.Type = "string"; // ✅ مهم جدًا
            schema.Format = null;   // ✅ علشان ما يفضلش integer
        }
    }
}
