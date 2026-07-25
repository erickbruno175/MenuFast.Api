namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa {
    public class HorarioFuncionamento {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeSpan HoraAbertura { get; set; }
        public TimeSpan HoraFechamento { get; set; }
        public bool Fechado { get; set; }
        public Empresa.Empresa? Empresa { get; set; }
    }
}