using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using MimeKit;

namespace MenuFast.Api.Api.Application.Services.Email {
    public class GoogleEmailService {
        private readonly IConfiguration _configuration;

        public GoogleEmailService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task EnviarAsync(string destinatario,string assunto,string mensagem) {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromFile(_configuration [ "GoogleEmail:CredentialsFile" ]!).Secrets,
                new [ ]
                {
                    GmailService.Scope.GmailSend
                },
                "MenuFast",
                CancellationToken.None);

            var gmailService = new GmailService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MenuFast"
                });

            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    _configuration [ "GoogleEmail:NomeRemetente" ],
                    _configuration [ "GoogleEmail:EmailRemetente" ]
                )
            );

            email.To.Add(
                MailboxAddress.Parse(destinatario)
            );

            email.Subject = assunto;

            email.Body = new BodyBuilder
            {
                HtmlBody = mensagem
            }.ToMessageBody();

            using var stream = new MemoryStream();

            email.WriteTo(stream);
            
            var raw  = Convert.ToBase64String(stream.ToArray());

            var message = new Message
            {
                Raw = raw
            };

            await gmailService.Users.Messages.Send(message,"me").ExecuteAsync();
        }
    }
}

