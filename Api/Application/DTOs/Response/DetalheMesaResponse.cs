using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Application.DTOs.Response {
    public class DetalheMesaResponse {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int LojaId { get; set; }
        public StatusMesa StatusMesa { get; set; }
    }
}
