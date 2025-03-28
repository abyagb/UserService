namespace UserService.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entity, Guid id)
            : base($"{entity} with id ‘{id}’ does not exist.") { }
    }
}

