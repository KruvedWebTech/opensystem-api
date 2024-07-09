namespace opensystem_api.Helpers
{
    public class ConstantsEnums
    {
        public static class Strings
        {
            public const string ErrorMessage = "An error occurred.";
            public const string SuccessMessage = "Operation successful.";
            public const string NOTFOUND = "User Not Found";
            public const string InvalidInput = "Invalid input provided.";
            public const string UnauthorizedAccess = "You do not have permission to access this resource.";
        }

            public static class Url
            {
                public const string ForgotPassword = "forgot-password";
                public const string CreateUser = "create-user";
                public const string DeleteUser = "delete-user/{id}";
                public const string Users = "users";
                public const string Login = "login";
                public const string Environment = "environment";
            }

            public static class Settings
        {
            public const string DatabaseConnectionString = "Server=myserver;Database=mydatabase;User Id=myuser;Password=mypassword;";
        }
    }
}
