using MenuFast.Api.Api.Domain.Entities.Models.Seguranca;
using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.SegurancaServices {
    public class ConfiguracoesSegurancaServices {
        private readonly MenuFastContext _context;

        public ConfiguracoesSegurancaServices(MenuFastContext context) {
            _context = context;
        }

        public async Task<ConfiguracaoSeguranca> ConsultarAsync(int lojaId) {
            var configuracao = await _context.ConfiguracoesSeguranca.FirstOrDefaultAsync(x => x.LojaId == lojaId);

            if(configuracao == null)
            {
                configuracao = new ConfiguracaoSeguranca{LojaId = lojaId};
                _context.ConfiguracoesSeguranca.Add(configuracao);
                await _context.SaveChangesAsync();
            }

            return configuracao;
        }

        public async Task<ConfiguracaoSeguranca> AtualizarAsync(int lojaId,int maxTentativasLogin,int tempoBloqueioMinutos,int tempoExpiracaoSessaoDias) {
            var configuracao = await _context.ConfiguracoesSeguranca
                .FirstOrDefaultAsync(x => x.LojaId == lojaId);

            if(configuracao == null)
            {
                configuracao = new ConfiguracaoSeguranca{LojaId = lojaId};
                _context.ConfiguracoesSeguranca.Add(configuracao);
            }
            configuracao.MaxTentativasLogin = maxTentativasLogin;
            configuracao.TempoBloqueioMinutos = tempoBloqueioMinutos;
            configuracao.TempoExpiracaoSessaoDias = tempoExpiracaoSessaoDias;
            await _context.SaveChangesAsync();

            return configuracao;
        }
    }
}