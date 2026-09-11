namespace MenuFast.Api.Api.Application.DTOs.Response {

    public class ConfiguracaoSegurancaResponse {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public int MaxTentativasLogin { get; set; }
        public int TempoBloqueioMinutos { get; set; }
        public int TempoExpiracaoSessaoDias { get; set; }
    }
}