using System.Text.RegularExpressions;

namespace App.WebApi.Infrastructure
{
    public class LowercaseUrls : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            return value == null ? null : Regex.Replace(value.ToString() ?? "", "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
