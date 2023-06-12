using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Configuration;
using System.IO;
using OpenPop.Pop3;

namespace BusinessLogicWhatsMyIp
{
	public class BusinessLogicSpam
	{
		public int sendCount;
		public Email email;
		public string EmailFilePath;
		public string DeleteMailsFilePath;
		public string LocalIP { get; set; }
		public string PublicIP { get; set; }

		public BusinessLogicSpam()
		{			
			email = new Email();
			ConfigureMailSettings();
		}

		public bool SendMail(string subject, string message)
		{
			email.Subject = (subject == String.Empty) ? "[OSJ] NUEVO Curso de Astronomía Observacional" : subject;
			email.Message = message;

			sendCount++;
			return MailUtils.SendEmail(email);
		}

		public void AddFailedEmailAddress(string emailAddress)
		{
			try
			{
				// Append the email address to the file
				using (StreamWriter writer = File.AppendText(DeleteMailsFilePath))
				{
					writer.Write(emailAddress + ";");
				}

				Console.WriteLine($"Email address {emailAddress} added to the file.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred while adding the email address to the file: " + ex.Message);
			}
		}


		public void ProcessFailedEmails(string deletePathFile)
		{
			DeleteMailsFilePath = deletePathFile;
			using (var client = new Pop3Client())
			{
				client.Connect(email.SMTPServer, email.POP3Port, false);
				client.Authenticate(email.From, email.Password);
				int emailCount = client.GetMessageCount();
				for (int i = emailCount; i > 0; i--)
				{
					Email receivedEmail = MailUtils.GetEmailByIndex(client,i, email.SMTPServer, email.POP3Port, email.From, email.Password);
					if (IsUndeliveredEmail(receivedEmail))
					{
						string failedAddress = MailUtils.ExtractEmailFromFailedSent(receivedEmail.Message);
						AddFailedEmailAddress(failedAddress);
					}
				}
			}
		}

        private bool IsUndeliveredEmail(Email receivedEmail)
        {
			if (receivedEmail.From.Contains("mailer-daemon@") && 
				!receivedEmail.Message.Contains("Domain observatoriosanjose.com.ar has exceeded the max defers and failures per hour") &&
				receivedEmail.Subject.Contains("Mail delivery failed") &&
				receivedEmail.Message.Contains("550 "))
				return true;
			return false;
        }

        private void ConfigureMailSettings()
		{
			email.From = ConfigurationManager.AppSettings["FromAddress"];
			email.To = ConfigurationManager.AppSettings["ToAddress"];
			email.SMTPClient = ConfigurationManager.AppSettings["SMTP"];
			email.SMTPServer = ConfigurationManager.AppSettings["SMTP"];
			email.SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
			email.POP3Port = Convert.ToInt32(ConfigurationManager.AppSettings["POP3Port"]);

			email.Password = ConfigurationManager.AppSettings["Password"];			
			email.Domain = ConfigurationManager.AppSettings["Domain"];
			email.IsSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"]);
			email.UseDefaultCredentials= bool.Parse(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
		}


		public string[] GetMailsFromFile(string file)
		{
			EmailFilePath = file;
			return MailUtils.GetMailsFromFile(file);
		}
	}
}
