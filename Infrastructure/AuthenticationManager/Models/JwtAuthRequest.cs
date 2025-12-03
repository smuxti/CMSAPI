namespace AuthenticationManager.Models
{
    public class JwtAuthRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int RoleTypeCode { get; set; }
        public string TenantCode { get; set; }
        public string MobileNumber { get; set; }
        public string Token { get; set; }
        public string SecurityKey { get; set; }
    }
}
