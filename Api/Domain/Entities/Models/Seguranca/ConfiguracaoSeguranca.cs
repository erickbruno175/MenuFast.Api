using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain.Entities.Models.Seguranca {
    public class ConfiguracaoSeguranca {

        public int Id { get; set; }
        public int LojaId { get; set; }
        public Loja.Loja Loja { get; set; }
        public int MaxTentativasLogin { get; set; } = 5;
        public int TempoBloqueioMinutos { get; set; } = 30;
        public int TempoExpiracaoSessaoDias { get; set; } = 60;
    }
}
