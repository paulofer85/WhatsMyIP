using System;
using System.Net.Mail;
using System.Net;
using System.IO;
using OpenPop.Pop3;
using OpenPop.Mime.Header;
using OpenPop.Mime;
using System.Text.RegularExpressions;
using Data;

namespace Common
{
    public static class MailUtils
	{
		public static bool IsValidEmail(string emailAddress)
		{
			try
			{
				MailAddress mailAddress = new MailAddress(emailAddress);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		public static bool SendEmail(Email email)
		{
			try
			{
				SmtpClient smtp = new SmtpClient
				{
					Host = email.SMTPServer,
					Port = email.SMTPPort,
					EnableSsl = false,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = email.UseDefaultCredentials,
					Credentials = new NetworkCredential(email.From, email.Password)
				};

				using (var message = new MailMessage(new MailAddress(email.From, "Observatorio San José"), new MailAddress(email.To,email.To))
				{
					Subject = email.Subject,
					Body = email.Message
				})
				{
					message.IsBodyHtml = true;
					smtp.Send(message);
				}

				return true;
			}
			catch (System.Net.Mail.SmtpException ex)
			{
				return false;
				throw ex;
			}			
		}



		public static bool SendEmail(Email email, string to)
		{
			try
			{
				SmtpClient smtp = new SmtpClient
				{
					Host = email.SMTPServer,
					Port = email.SMTPPort,
					EnableSsl = false,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = email.UseDefaultCredentials,
					Credentials = new NetworkCredential(email.From, email.Password)
				};

				using (var message = new MailMessage(new MailAddress(email.From, "Observatorio San José"), new MailAddress(to, to))
				{
					Subject = email.Subject,
					Body = email.Message
				})
				{
					message.IsBodyHtml = true;
					smtp.Send(message);
				}

				return true;
			}
			catch (System.Net.Mail.SmtpException ex)
			{
				return false;
			}
		}


		public static int GetEmailCount(string host, int port, string username, string password)
		{
			using (var client = new Pop3Client())
			{
                try
                {
					client.Connect(host, port, false);
					client.Authenticate(username, password);

					return client.GetMessageCount();
				}
                catch (Exception ex)
                {
					throw new Exception("Unable to get count of mails");
				}

			}
		}


		public static Email GetLastEmail(string host, int port, string username, string password)
		{
			var emailRdo = new Email();
			using (var client = new Pop3Client())
			{
				client.Connect(host, port, false);
				client.Authenticate(username, password);

				int messageCount = client.GetMessageCount();
				if (messageCount == 0)
				{
					throw new Exception("No emails found in the account.");
				}

				OpenPop.Mime.Message lastMessage = client.GetMessage(messageCount);

				// Retrieve the subject
				emailRdo.Subject = lastMessage.Headers.Subject;
				
				// Retrieve the sender
				RfcMailAddress sender = lastMessage.Headers.From;
				emailRdo.From = sender.Address.ToLower();

				// Retrieve the recipient
				RfcMailAddress recipient = lastMessage.Headers.To[0];
				emailRdo.To = recipient.Address;

				// Retrieve the body
				MessagePart body = lastMessage.FindFirstPlainTextVersion();
				if (body != null)
				{
					string bodyText = body.GetBodyAsText();
					emailRdo.Message = bodyText;
				}
			}
			return emailRdo;
		}

		public static Email GetEmailByIndex(int indexMail, string host, int port, string username, string password)
		{
			var emailRdo = new Email();
			using (var client = new Pop3Client())
			{
				client.Connect(host, port, false);
				client.Authenticate(username, password);

				OpenPop.Mime.Message lastMessage = client.GetMessage(indexMail);

				// Retrieve the subject
				emailRdo.Subject = lastMessage.Headers.Subject;

				// Retrieve the sender
				RfcMailAddress sender = lastMessage.Headers.From;
				emailRdo.From = sender.Address.ToLower();

				// Retrieve the recipient
				RfcMailAddress recipient = lastMessage.Headers.To[0];
				emailRdo.To = recipient.Address;

				// Retrieve the body
				MessagePart body = lastMessage.FindFirstPlainTextVersion();
				if (body != null)
				{
					string bodyText = body.GetBodyAsText();
					emailRdo.Message = bodyText;
				}
			}
			return emailRdo;
		}

		public static Email GetEmailByIndex(Pop3Client client, int indexMail, string host, int port, string username, string password)
		{
			var emailRdo = new Email();

			OpenPop.Mime.Message lastMessage = client.GetMessage(indexMail);

			// Retrieve the subject
			emailRdo.Subject = lastMessage.Headers.Subject;

			// Retrieve the sender
			RfcMailAddress sender = lastMessage.Headers.From;
			emailRdo.From = sender.Address.ToLower();

			// Retrieve the recipient
			RfcMailAddress recipient = lastMessage.Headers.To[0];
			emailRdo.To = recipient.Address;

			// Retrieve the body
			MessagePart body = lastMessage.FindFirstPlainTextVersion();
			if (body != null)
			{
				string bodyText = body.GetBodyAsText();
				emailRdo.Message = bodyText;
				}
			return emailRdo;
		}

		public static string ExtractEmailFromFailedSent(string input)
		{
			string pattern = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}";

			Match match = Regex.Match(input, pattern);
			if (match.Success && IsValidEmail(match.Value))
				return match.Value;

			return null;
		}


		public static string[] GetMailsFromFile(string file)
		{
			//se levantan los mails del archivo
			return File.ReadAllLines(file); 
		}

		public static string[] GetMailsFromFile(string file, char splitter)
		{
			//se levantan los mails del archivo
			return File.ReadAllText(@file).Split(splitter);
		}


	}
}
