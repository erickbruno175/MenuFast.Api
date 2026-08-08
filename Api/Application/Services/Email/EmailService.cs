using MenuFast.Api.Api.Domain.Enum;
using Microsoft.OpenApi;
using System.Net;
using System.Net.Mail;

namespace MenuFast.Api.Api.Application.Services.Email;

public class EmailService {
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IConfiguration configuration, ILogger<EmailService> logger) {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task EnviarAsync(string destinatario, string assunto, string mensagem) {

        try
        {

            var host = _configuration [ "Email:Host" ] ?? throw new InvalidOperationException("Email:Host não configurado.");
            var port = int.Parse(_configuration [ "Email:Port" ] ?? throw new InvalidOperationException("Email:Port não configurado."));
            var emailRemetente = _configuration [ "Email:EmailRemetente" ] ?? throw new InvalidOperationException("Email:EmailRemetente não configurado.");
            var nomeRemetente = _configuration [ "Email:NomeRemetente" ] ?? throw new InvalidOperationException("Email:NomeRemetente não configurado.");
            var senha = _configuration [ "Email:Senha" ] ?? throw new InvalidOperationException("Email:Senha não configurado.");

            using var smtp = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(emailRemetente,senha)
            };

            using var email = new MailMessage
            {
                From = new MailAddress(emailRemetente,nomeRemetente),
                Subject = assunto,
                Body = mensagem,
                IsBodyHtml = true
            };

            email.To.Add(destinatario);
            await smtp.SendMailAsync(email);
        }
        catch(Exception ex)
        {
            
                _logger.LogError(ex, "Erro ao enviar e-mail para {Destinatario}.", destinatario , $"Tipo de log:{TipoLog.ErroEnvioEmail.GetDisplayName()}" 
                , "Data: {DateTime.UtcNow}");
               throw new InvalidOperationException($"Erro ao enviar e-mail para {destinatario}.", ex);

        }
      
    }
}