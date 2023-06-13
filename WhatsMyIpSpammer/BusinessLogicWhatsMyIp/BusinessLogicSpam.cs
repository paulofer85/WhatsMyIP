using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Configuration;
using System.IO;
using OpenPop.Pop3;
using Newtonsoft.Json;

namespace BusinessLogicWhatsMyIp
{
	public class BusinessLogicSpam
	{
		public int sendCount;
		public Email email;
		public string EmailFilePath;
		public string DeleteMailsFilePath;
		public DateTime lastSendTime;
		public List<Adress> adresses;
		public String[] deleteMails;
        public Course[] courses;

		public string LocalIP { get; set; }
		public string PublicIP { get; set; }

		public BusinessLogicSpam()
		{
			email = new Email();             
			ConfigureMailSettings();
			adresses = GetMailsFromJson("MailsBD.json");
			deleteMails = GetMailsFromFile("sacar.txt");
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
			string[] rdo;
			EmailFilePath = file;
			rdo = MailUtils.GetMailsFromFile(file);
			if (rdo.Length > 1) return rdo;
			return MailUtils.GetMailsFromFile(file, ';');
		}

		public void UpdateMailsWithBounced(string mailsFile, string bouncedFile)
		{
			string[] mails = MailUtils.GetMailsFromFile(mailsFile);
			string[] bounceds = MailUtils.GetMailsFromFile(bouncedFile);
			string fileBackup = $"{mailsFile.Split('.').First()}{DateTime.Now.ToString("yyyyMMdd")}.txt";
			File.WriteAllLines(fileBackup, mails);

			List<string> updatedMails = new List<string>(mails);
			foreach (string bounced in bounceds)
			{
				if (updatedMails.Contains(bounced))
					updatedMails.Remove(bounced);
			}

			File.WriteAllLines(mailsFile, updatedMails);

			Console.WriteLine("Mails updated successfully.");
		}

		public void GenerateJSONFromFile(string file)
		{
			List<string> emailAddresses = GetMailsFromFile(file).ToList();

			List<Dictionary<string, string>> jsonObjects = new List<Dictionary<string, string>>();

			foreach (string email in emailAddresses)
			{
				Dictionary<string, string> jsonObject = new Dictionary<string, string>();
				jsonObject["mail"] = email;
				jsonObject["name"] = GenerateName(email);
				jsonObjects.Add(jsonObject);
			}

			string json = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);
			File.WriteAllText("output.json", json);
			Console.WriteLine("Generated JSON BD from plain text file");

		}

		public string GenerateName(string email)
		{
			string[] parts = email.Split('@');
			string username = parts[0];

			// Remove non-alphanumeric characters from the username
			username = new string(username.ToCharArray()
				.Where(c => char.IsLetterOrDigit(c) || c == '.')
				.ToArray());

			// Split the username into words
			string[] words = username.Split('.');

			// Process each word in the username
			for (int i = 0; i < words.Length; i++)
			{
				string word = words[i];
				if (!String.IsNullOrEmpty(word)) { 
					// Check if the word is an abbreviation
					if (word.Length == 2 && char.IsUpper(word[0]) && char.IsUpper(word[1]))
					{
						words[i] = word.ToUpper();
					}
					else
					{
						// Capitalize the first letter of the word and make the rest lowercase
						words[i] = word[0].ToString().ToUpper() + word.Substring(1).ToLower();
					}
				}
			}

			// Combine the words to form the name
			string name = string.Join(" ", words);

			return name;
		}

		public List<Adress> GetMailsFromJson(string filePath)
		{
			// Read the JSON file
			string json = File.ReadAllText(filePath);

			// Deserialize the JSON array into a list of Person objects
			List<Adress> people = JsonConvert.DeserializeObject<List<Adress>>(json);

			// Filter out people with duplicate emails
			List<Adress> uniquePeople = people.GroupBy(p => p.Mail)
											  .Select(g => g.First())
											  .ToList();
			return uniquePeople;
		}

		public Course[] GetCourses(string coursesFile)
		{
			string json = File.ReadAllText(coursesFile);
			return JsonConvert.DeserializeObject<Course[]>(json);
		}

		public void ProcessMails(Course course)
		{
			try
			{
				int cont = 0;
				System.Console.WriteLine($"WhatsMyIPSpammer: Se encontraron un total de {adresses.Count} mails unicos.");
				while (cont < adresses.Count)
				{
					if (sendCount < 150)
					{
						lastSendTime = DateTime.Now;
						if (!deleteMails.Contains(adresses[cont].Mail) && adresses[cont].Mail != String.Empty)
						{
							SendMail(course.Subject, course.Message);
							System.Console.WriteLine($"{DateTime.Now.ToString("yyMMdd HH:mm:ss")} | envió mail nro {cont+1}/{adresses.Count} mail {adresses[cont].Mail}");
						}
						cont++;
					}
					else if (DateTime.Now > lastSendTime.AddHours(1))
					{
						sendCount = 0;
						System.Console.WriteLine("WhatsMyIPSpammer: se cumplio la hora luego de haber enviado lote de mails");
					}
					System.Threading.Thread.Sleep(5000);
				}
				System.Console.ReadKey();
			}
			catch (Exception x)
			{
				System.Console.WriteLine(new System.IO.ErrorEventArgs(x).ToString());
			}
		}

		public void ProcessMails(string subject, string message)
		{
			try
			{
				int cont = 0;
				while (cont < adresses.Count)
				{
					if (sendCount < 150)
					{
						lastSendTime = DateTime.Now;
						if (!deleteMails.Contains(adresses[cont].Mail) && adresses[cont].Mail != String.Empty)
						{
							SendMail(subject, message);

							System.Console.WriteLine("WhatsMyIPSpammer: se envio el mail nro " + cont + " mail " + adresses[cont].Mail);
						}
						cont++;
					}
					else if (DateTime.Now > lastSendTime.AddHours(1))
					{
						sendCount = 0;
						System.Console.WriteLine("WhatsMyIPSpammer: se cumplio la hora luego de haber enviado lote de mails");
					}
					System.Threading.Thread.Sleep(5000);
				}
				System.Console.ReadKey();
			}
			catch (Exception x)
			{
				System.Console.WriteLine(new System.IO.ErrorEventArgs(x).ToString());
			}
		}
	}
}
