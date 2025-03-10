namespace UserService.Application.Exceptions
{
    public class InvalidEntityException : Exception
    {
        public InvalidEntityException(string entityName, Guid? id, string? errorMessage)
           : base($"An error occured with Entity : '{entityName}'. EntityId: '{id ?? null}'. ErrorMessage : {errorMessage}'")
        {
        }
    }
}
