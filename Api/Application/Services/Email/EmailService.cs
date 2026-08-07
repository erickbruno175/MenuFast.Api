using MenuFast.Api.Api.Domain.Constantes;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.Email {
    public class EmailService {

        private readonly GoogleEmailService _emailService;
        private readonly MenuFastContext _menuFastContext;
        private readonly ILogger<EmailService> _logger;

        public EmailService(GoogleEmailService emailService, MenuFastContext menuFastContext, ILogger<EmailService> logger) {
            _emailService = emailService;
            _menuFastContext = menuFastContext;
        }
        public async Task EnviarEmailRecuperacaoSenha(string email) {
            try
            {

                var funcionario = await _menuFastContext.Funcionarios.FirstAsync(f => f.Email == email);
                if(funcionario == null) { throw new BusinessLogicException("E-mail  não encontrado."); }
                var template = await _menuFastContext.TemplatesEmail.FirstAsync(t => t.Nome == "RECUPERAÇÃO DE SENHA");
                var corpo = template.Conteudo.Replace("{NOME}", funcionario.Nome).Replace("{LINK_RECUPERACAO}", LinkEmail.LinkRecuperarSenha);
                await _emailService.EnviarAsync(email, template.Assunto, corpo);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex, "Erro ao enviar e-mail de recuperação de senha para {Email}",
                    email, $"Data de tentativa: {DateTime.Now}");
                throw new BusinessLogicException("Erro ao enviar e-mail de recuperação de senha.");
            }
            finally
            {
                await _menuFastContext.DisposeAsync();// disposa o contexto do banco de dados para liberar recursosz
            }
        }

        public async Task EnviarEmailConfirmacaoCadastro(string email) {
            try
            {
                var funcionario = await _menuFastContext.Funcionarios.FirstAsync(f => f.Email == email);
                if(funcionario == null) { throw new BusinessLogicException("E-mail  não encontrado."); }
                var template = await _menuFastContext.TemplatesEmail.FirstAsync(t => t.Nome == "CONFIRMAÇÃO DE CADASTRO");
                var corpo = template.Conteudo.Replace("{NOME}", funcionario.Nome).Replace("{LINK_CONFIRMAR_EMAIL}", LinkEmail.LinkConfirmarEmail);
                await _emailService.EnviarAsync(email, template.Assunto, corpo);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex, "Erro ao enviar e-mail de confirmação de cadastro para {Email}",
                    email, $"Data de tentativa: {DateTime.Now}");
                throw new BusinessLogicException("Erro ao enviar e-mail de confirmação de cadastro.");
            }
            finally
            {
                await _menuFastContext.DisposeAsync();// disposa o contexto do banco de dados para liberar recursos
            }
        }


    }
}
