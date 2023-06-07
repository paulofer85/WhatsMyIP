using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Common
{
	public class SMTPEmail
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
	}

	public static class MailUtils
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		public static bool SendEmail(SMTPEmail email)
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



		public static bool SendEmail(SMTPEmail email, string to)
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
				throw ex;
			}
		}


		public static string[] GetMailsFromFile(string file)
		{
			//se levantan los mails del archivo
			return File.ReadAllText(@file).Split(';');
		}


	}
}
