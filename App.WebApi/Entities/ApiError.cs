namespace App.WebApi.Entities
{
    public class ApiError
    {
        public string code { get; private set; }

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string message { get; private set; }

        public ApiError(string code, string message = "")
        {
            this.code = code;
            this.message = message;
        }
    }
}
