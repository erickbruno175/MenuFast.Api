using AutoMapper;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Responses.Menu;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Domain.Entities.Models.Cliente;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Mesa;
using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using MenuFast.Api.Api.Domain.Entities.Models.Seguranca;

namespace MenuFast.Api.Api.Mappings {
    public class MappingsProfile : Profile {
        public MappingsProfile() {
            CreateMap<Cliente, ClienteResponse>();
            CreateMap<ConfiguracaoSeguranca, ConfiguracoesLojaResponse>();
            CreateMap<Mesa, DetalheMesaResponse>();
            CreateMap<Produto, DetalheProdutosResponse>();
            CreateMap<CategoriaProduto, CategoriaResponse>();
            CreateMap<CategoriaProduto, AlertaEstoqueResponse>();
            CreateMap<Pedido, PedidoResponse>();
            CreateMap<ItemPedido, ItemPedidoResponse>();
            CreateMap<Pedido, PedidoProducaoResponse>();
            CreateMap<FormaPagamento, FormaPagamentoResponse>();
            CreateMap<ItemPedido, ItemPedidoProducaoResponse>().ForMember(dest => dest.ItemPedidoId,opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NomeProduto,opt => opt.MapFrom(src => src.Produto.Nome));
        }
    }
}