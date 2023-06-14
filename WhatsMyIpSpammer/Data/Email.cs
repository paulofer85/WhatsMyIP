using System.Net.Mail;

namespace Data
{
    public class Email
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Message { get; set; }
		public string Subject { get; set; }
		public MailAddressCollection mailAddresses = new MailAddressCollection();
		public string SMTPServer { get; set; }
		public string SMTPClient { get; set; }
		public int SMTPPort { get; set; }
		public string Password{ get; set; }
		public string Domain{ get; set; }
		public bool IsSSL{ get; set; }
		public bool UseDefaultCredentials{ get; set; }
        public int POP3Port { get; set; }
    }
}
