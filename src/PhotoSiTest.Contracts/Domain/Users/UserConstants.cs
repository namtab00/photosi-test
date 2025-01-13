namespace PhotoSiTest.Contracts.Domain.Users;

public class UserConstants
{
    public const int EmailMaxLength = 256;

    public const int FirstNameMaxLength = 100;

    public const int LastNameMaxLength = 100;


    public static class ApiRoutes
    {
        public const string Create = "/users";

        public const string Delete = "/users/{id}";

        public const string GetAll = "/users";

        public const string GetByEmail = "/users/by-email";

        public const string GetById = "/users/{id}";

        public const string Update = "/users/{id}";
    }
}
