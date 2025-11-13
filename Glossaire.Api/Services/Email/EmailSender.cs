using MailKit.Net.Smtp;
using MimeKit;

namespace TechQuiz.Api.Services.Email
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var settings = _config.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TechQuiz", settings["Username"]));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            using var client = new SmtpClient();
            
            if (bool.Parse(settings["UseSSL"]!))
                await client.ConnectAsync(settings["Host"], int.Parse(settings["Port"]!), true);
            else
                await client.ConnectAsync(settings["Host"], int.Parse(settings["Port"]!), false);

            await client.AuthenticateAsync(settings["Username"], settings["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
