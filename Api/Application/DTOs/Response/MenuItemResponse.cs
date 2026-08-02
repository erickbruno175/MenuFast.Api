namespace MenuFast.Api.Api.Application.Responses.Menu;

public class MenuItemResponse {
    public string Nome { get; set; } = string.Empty;
    public string? Icone { get; set; }
    public string? Rota { get; set; }
    public string? Permissao { get; set; }

    public List<MenuItemResponse> Filhos { get; set; } = [ ];
}