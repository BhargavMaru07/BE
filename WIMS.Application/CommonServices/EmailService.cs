using Microsoft.Extensions.Configuration;
using WIMS.Application.Interfaces.Common;
using DnsClient;
using MailKit.Net.Smtp;
using MimeKit;

namespace WIMS.Application.CommonServices;


public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var senderName = _configuration["EmailSettings:SenderName"] ?? string.Empty;
            var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? throw new InvalidOperationException("SenderEmail is not configured.");
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new InvalidOperationException("SmtpServer is not configured.");
            var senderPassword = _configuration["EmailSettings:SenderPassword"] ?? throw new InvalidOperationException("SenderPassword is not configured.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            client.CheckCertificateRevocation = true;

            await client.ConnectAsync(smtpServer, 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(senderEmail, senderPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"EMAIL ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> IsEmailDomainValidAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        var formatAttr = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (!formatAttr.IsValid(email)) return false;

        try
        {
            var domain = email.Split('@')[1];
            var lookup = new LookupClient();
            var result = await lookup.QueryAsync(domain, QueryType.MX);
            return result.Answers.MxRecords().Any();
        }
        catch
        {
            return false;
        }
    }

}
