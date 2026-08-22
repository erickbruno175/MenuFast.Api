using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using System.Security.Claims;

namespace MenuFast.Api.Api.Application.Services.ContextUser {
    public class UsuarioContextService {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioContextService(
            IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        private ClaimsPrincipal? Usuario => _httpContextAccessor.HttpContext?.User;
        public int? FuncionarioId() {var valor = Usuario?.FindFirst("funcionarioId")?.Value;return int.TryParse(valor, out var funcionarioId)? funcionarioId: null;}
        public string? Nome() { return Usuario?.FindFirst("nome")?.Value; }
        public string? Login() { return Usuario?.FindFirst(ClaimTypes.Name)?.Value; }
        public string? Perfil() { return Usuario?.FindFirst("perfil")?.Value; }

        public int? LojaId() {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("lojaId");
            if(claim == null)return null;
            if(int.TryParse(claim.Value, out var lojaId))return lojaId;
            return null;
        }
    }
}
