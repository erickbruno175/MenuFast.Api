namespace MenuFast.Api.Api.Domain.Entities.Models.Empresa {
    public class HorarioFuncionamento {
        public Guid Id { get; set; }
        public Guid EmpresaId { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeSpan HoraAbertura { get; set; }
        public TimeSpan HoraFechamento { get; set; }
        public bool Fechado { get; set; }
        public Empresa? Empresa { get; set; }
    }
}