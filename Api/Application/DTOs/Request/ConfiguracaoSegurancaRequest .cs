namespace MenuFast.Api.Api.Application.DTOs.Request {
    public class ConfiguracaoSegurancaRequest {
        public int MaxTentativasLogin { get; set; }
        public int TempoBloqueioMinutos { get; set; }
        public int TempoExpiracaoSessaoDias { get; set; }
    }
}
