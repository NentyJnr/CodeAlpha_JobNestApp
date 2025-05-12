namespace JobNest.Dtos.Auth.Requests
{
    public class CurrentUser
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
