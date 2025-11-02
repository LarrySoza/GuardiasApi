using System.Text.Json;

namespace App.WebApi.Models.Shared
{
    public class ApiException : Exception
    {
        public string message { get { return base.Message; } }

        public ApiError[] Errors { get; private set; }

        public ApiException(string message, params ApiError[] errors) : base(message)
        {
            Errors = errors;
        }

        public override string ToString() => JsonSerializer.Serialize(new { message, errors = Errors });

    }
}
