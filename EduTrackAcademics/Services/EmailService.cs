using System.Net;
using System.Net.Mail;

namespace EduTrackAcademics.Services
{
	public class EmailService
	{
		public void SendPassword(string toEmail, string password)
		{
			var mail = new MailMessage();
			mail.To.Add(toEmail);
			mail.Subject = "Coordinator Account Created";
			mail.Body =
$@"Your coordinator account has been created.
 
Temporary Password: {password}
 
Please login and change your password immediately.";

			mail.From = new MailAddress("admin@edutrack.com");

			var smtp = new SmtpClient("smtp.gmail.com", 587)
			{
				Credentials = new NetworkCredential("saisriramyasetti@gmail.com", "uync xqmp xvre guqw"),
				EnableSsl = true
			};

			smtp.Send(mail);
		}
	}
}