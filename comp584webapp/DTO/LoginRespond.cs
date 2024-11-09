namespace comp584webapp.DTO
{
    public class LoginRespond
    {
        public bool Success { get; set; }
        public required string Message { get; set; }

        public required string Token { get; set; }
    }
}
