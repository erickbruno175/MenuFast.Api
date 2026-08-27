using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Domain.Entities.Models.Mesa;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.MesaServices;

public class MesaService {
    private readonly MenuFastContext _context;

    public MesaService(MenuFastContext context) {
        _context = context;
    }
    public async Task<DetalheMesaResponse> CadastrarMesa(MesaRequest request) {
        var numero = request.Numero.Trim();

        var jaExiste = await _context.Mesas
            .AnyAsync(x =>x.LojaId == request.LojaId && x.Numero.ToUpper() == numero.ToUpper());

        if(jaExiste)throw new BusinessLogicException("Essa mesa já está cadastrada.");

        var mesa = new Mesa
        {
            Numero = numero,
            LojaId = request.LojaId,
            StatusMesa = StatusMesa.Livre
        };
        await _context.Mesas.AddAsync(mesa);
        await _context.SaveChangesAsync();
        return ConverterParaDetalhe(mesa);
    }

    public async Task<List<DetalheMesaResponse>> ListarMesas(int lojaId) {
        return await _context.Mesas.AsNoTracking()
            .Where(x => x.LojaId == lojaId)
            .OrderBy(x => x.Numero)
            .Select(x => new DetalheMesaResponse
            {
                Id = x.Id,
                Numero = x.Numero,
                LojaId = x.LojaId,
                StatusMesa = x.StatusMesa
            })
            .ToListAsync();
    }

    public async Task<DetalheMesaResponse> DetalharMesa(int idMesa) {
        var mesa = await _context.Mesas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idMesa);

        if(mesa == null)throw new BusinessLogicException("Mesa não encontrada.");
        return ConverterParaDetalhe(mesa);
    }

    public async Task<DetalheMesaResponse> AtualizarMesa(int idMesa,MesaRequest request) {
        var mesa = await _context.Mesas.FirstOrDefaultAsync(x => x.Id == idMesa);

        if(mesa == null)throw new BusinessLogicException("Mesa não encontrada.");
        var numero = request.Numero.Trim();

        var jaExiste = await _context.Mesas.AnyAsync(x =>
                x.Id != idMesa &&
                x.LojaId == request.LojaId &&
                x.Numero.ToUpper() == numero.ToUpper());

        if(jaExiste)throw new BusinessLogicException("Já existe outra mesa com esse número.");
        mesa.Numero = numero;
        mesa.LojaId = request.LojaId;
        await _context.SaveChangesAsync();
        return ConverterParaDetalhe(mesa);
    }

    public async Task<DetalheMesaResponse> AlterarStatusMesa(int idMesa,StatusMesa status) {
        var mesa = await _context.Mesas.FirstOrDefaultAsync(x => x.Id == idMesa);

        if(mesa == null)throw new BusinessLogicException("Mesa não encontrada.");

        mesa.StatusMesa = status;
        await _context.SaveChangesAsync();
        return ConverterParaDetalhe(mesa);
    }

    public async Task RemoverMesa(int idMesa) {var mesa = await _context.Mesas.FirstOrDefaultAsync(x => x.Id == idMesa);

        if(mesa == null)throw new BusinessLogicException("Mesa não encontrada.");

        var possuiPedidos = await _context.Pedidos.AnyAsync(x => x.MesaId == idMesa);

        if(possuiPedidos)throw new BusinessLogicException("Não é possível remover uma mesa que possui pedidos.");
        _context.Mesas.Remove(mesa);
        await _context.SaveChangesAsync();
    }
    private static DetalheMesaResponse ConverterParaDetalhe(Mesa mesa) {
        return new DetalheMesaResponse
        {
            Id = mesa.Id,
            Numero = mesa.Numero,
            LojaId = mesa.LojaId,
            StatusMesa = mesa.StatusMesa
        };
    }
}