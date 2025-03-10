using UserService.Domain;

namespace UserService.Application.Helpers
{
    public static class EndUserServiceHelper
    {
        public static void ValidatePatchFields(Dictionary<string, object> fieldsToUpdate)
        {
            foreach (var field in fieldsToUpdate)
            {
                if (!ValidFields.Contains(field.Key))
                {
                    throw new ArgumentException($"Field ‘{ field.Key }’ is not a valid field for User.");
                }
            }
        }

        public static void ApplyPatch(User user, Dictionary<string, object> fieldsToUpdate)
        {
            var userClassType = typeof(User);
            foreach (var field in fieldsToUpdate)
            {
                var property = userClassType.GetProperty(field.Key) ?? throw new ArgumentException($"Field {field.Key} does not exist on {userClassType.Name}");
                property.SetValue(user, Convert.ChangeType(field.Value, property.PropertyType));
            }
        }

        private static readonly HashSet<string> ValidFields = typeof(User)
                                                              .GetProperties()
                                                              .Select(p => p.Name)
                                                              .ToHashSet();
    }
}
