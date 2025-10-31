using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Application.Configurations.Routing;

public sealed class ApiPrefixConvention : IApplicationModelConvention
{
    private readonly string _prefix;

    public ApiPrefixConvention(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("Prefix must not be empty", nameof(prefix));

        _prefix = prefix.Trim().Trim('/');
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel is null)
                {
                    selector.AttributeRouteModel = new AttributeRouteModel(
                        new RouteAttribute($"{_prefix}/[controller]"));
                    continue;
                }

                var template = selector.AttributeRouteModel.Template?.TrimStart('/') ?? string.Empty;
                if (template.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (template.StartsWith("api/", StringComparison.OrdinalIgnoreCase))
                    template = template[4..];

                selector.AttributeRouteModel.Template = string.IsNullOrWhiteSpace(template)
                    ? _prefix
                    : $"{_prefix}/{template}";
            }
        }
    }
}
