namespace Crefinso.DTOs
{
    public class UserSession
    {
        public string UserName { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string UserRole { get; set; } = null!;
    }

    public class UserResponse
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string UserRole { get; set; } = null!;
    }
    public class UserRequest
    {
        public string UserName { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string UserRole { get; set; } = null!;
    }
}
