namespace PetTracker.Server.Data
{
    public class EmailConfiguration
    {
        public const string EmailConfig = "EmailConfiguration";

        public string? SenderName { get; set; }
        public string? From { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
