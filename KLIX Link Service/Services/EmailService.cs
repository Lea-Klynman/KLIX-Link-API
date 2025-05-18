
using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.IServices;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MailKit.Search;
using MailKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Net.Imap;
using System.Threading;

namespace KLIX_Link_Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

  

public async Task<List<EmailMessage>> GetUnreadEmailsAsync()
        {
            var emails = new List<EmailMessage>();

            using var client = new ImapClient();
            await client.ConnectAsync(configuration["IMAP_SERVER"], int.Parse(configuration["IMAP_PORT"]), SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(configuration["GOOGLE_USER_EMAIL"], configuration["APP_PASSWORD"]);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);

            var results = await inbox.SearchAsync(SearchQuery.NotSeen);

            foreach (var uid in results)
            {
                var message = await inbox.GetMessageAsync(uid);
                emails.Add(new EmailMessage
                {
                    Id = (int)uid.Id,
                    From = message.From.ToString(),
                    Subject = message.Subject,
                    Body = message.TextBody ?? message.HtmlBody,
                    IsRead = false ,
                    Date = message.Date.DateTime,
                });
            }

            await client.DisconnectAsync(true);
            return emails;
        }

        public async Task<bool> SendEmailAsync(EmailRequest request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration["GOOGLE_USER_EMAIL"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart("plain") { Text = request.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(configuration["SMTP_SERVER"], int.Parse(configuration["SMTP_PORT"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(configuration["GOOGLE_USER_EMAIL"], configuration["APP_PASSWORD"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return true;
        }

        public async Task<bool> MarkEmailAsReadAsync(int emailId)
        {
            using var client = new ImapClient();
            await client.ConnectAsync(configuration["IMAP_SERVER"], int.Parse(configuration["IMAP_PORT"]), SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(configuration["GOOGLE_USER_EMAIL"], configuration["APP_PASSWORD"]);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);

            var uid = new UniqueId((uint)emailId);
            var messageUids = await inbox.SearchAsync(SearchQuery.All);  

            if (messageUids.Contains(uid)) 
            {
                await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                await client.DisconnectAsync(true);
                return true;
            }

            await client.DisconnectAsync(true);
            return false;
        }

        public async Task<bool> DeleteEmailAsync(int emailId)
        {
            using var client = new ImapClient();
            await client.ConnectAsync(configuration["IMAP_SERVER"], int.Parse(configuration["IMAP_PORT"]), SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(configuration["GOOGLE_USER_EMAIL"], configuration["APP_PASSWORD"]);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);

            var uid = new UniqueId((uint)emailId);
            var messageUids = await inbox.SearchAsync(SearchQuery.All);

            if (messageUids.Contains(uid))
            {
                await inbox.AddFlagsAsync(uid, MessageFlags.Deleted, true);
                await inbox.ExpungeAsync(); 
                await client.DisconnectAsync(true);
                return true;
            }

            await client.DisconnectAsync(true);
            return false;
        }

    }
}

