namespace WIMS.Application.Interfaces.Common;

public interface IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string body);
    public Task<bool> IsEmailDomainValidAsync(string email);
}
