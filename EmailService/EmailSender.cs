namespace EmailService;
using System.Net;
using System.Net.Mail;
public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.office365.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("jobamatm@gmail.com", "Jobama12")
        };
 
        return client.SendMailAsync(
            new MailMessage(from: "jobamatm@gmail.com",
                            to: email,
                            subject,
                            "Molim vas potvrdite vaš email: "+message
                            ));
    }
}