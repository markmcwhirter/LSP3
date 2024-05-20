namespace LSP3.Model
{
    public class AppSettings
    {
        public string? HostUrl { get; set; }
        public string? ImageData { get; set; }


		public string? SmtpServer { get; set; }
		public string? SmtpPort { get; set; }

		public string? SmtpFromAddress { get; set; }
        public string? SmtpToAddress { get; set; }

        public string? SmtpUser { get; set; }

		public string? SmtpPassword { get; set; }
	}
}
