namespace UserService.Application.DTOs
{
    public class UserPatchDto
    {
        public Guid UserId { get; init; }
        public required Dictionary<string, object> FieldsToUpdate { get; init; }
    }
}
