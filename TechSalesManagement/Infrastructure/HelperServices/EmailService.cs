using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TechSalesManagement.Application.Common.Configurations;
using TechSalesManagement.Application.HelperServices;

namespace TechSalesManagement.Infrastructure.HelperServices;

public class EmailService : IEmailService
{
    private readonly MailSettingsCO _mailSettings;

    public EmailService(IOptions<MailSettingsCO> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    private async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSettings.displayName, _mailSettings.fromEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            // Connect securely using MailKit logic
            await smtp.ConnectAsync(_mailSettings.host, _mailSettings.port, MailKit.Security.SecureSocketOptions.StartTls);
            
            if (!string.IsNullOrEmpty(_mailSettings.password))
            {
                await smtp.AuthenticateAsync(_mailSettings.userName, _mailSettings.password);
            }
            
            await smtp.SendAsync(email);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }

    public async Task SendVerificationEmailAsync(string to, string verificationLink)
    {
        string subject = "TechSales - Account Verification";
        string body = $@"
            <h3>Welcome to TechSales,</h3>
            <p>Thank you for registering. Please use this link (or enter the provided OTP) to verify your account:</p>
            <p><a href='{verificationLink}' style='background-color:#007bff;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>Click here to verify</a></p>
            <p>Alternatively, you can copy and paste this URL into your browser:</p>
            <p>{verificationLink}</p>
            <br/>
            <p>Best regards,</p>
            <p>The TechSales Team</p>
        ";
        
        await SendEmailAsync(to, subject, body);
    }

    public async Task SendPasswordResetEmailAsync(string to, string resetLink)
    {
        string subject = "TechSales - Password Reset Request";
        string body = $@"
            <h3>Hello,</h3>
            <p>We received a request to reset your TechSales account password.</p>
            <p><a href='{resetLink}' style='background-color:#28a745;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>Reset My Password</a></p>
            <p>If you did not request a password reset, please ignore this email.</p>
            <br/>
            <p>Best regards,</p>
            <p>The TechSales Team</p>
        ";
        
        await SendEmailAsync(to, subject, body);
    }
}
