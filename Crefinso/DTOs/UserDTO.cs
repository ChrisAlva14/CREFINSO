namespace Crefinso.DTOs
{
    public class UserSession
    {
        public string UserName { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string UserRole { get; set; } = null!;

        public string Name { get; set; } = null!;
    }

    public class LoginResponse
    {
        public string Token { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string Name { get; set; } = null!;
    }

    public class UserResponse
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string UserRole { get; set; } = null!;

        public string Name { get; set; } = null!;
    }

    public class UserRequest
    {
        public string UserName { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string UserRole { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
