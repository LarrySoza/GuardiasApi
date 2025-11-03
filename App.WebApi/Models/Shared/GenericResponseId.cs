namespace App.WebApi.Models.Shared
{
    public class GenericResponseId<T>
    {
        public T id { get; private set; }

        public GenericResponseId(T id)
        {
            this.id = id;
        }
    }
}
