namespace App.WebApi.Models.Shared
{
    public class GenericResponseIdDto<T>
    {
        public T id { get; private set; }

        public GenericResponseIdDto(T id)
        {
            this.id = id;
        }
    }
}
