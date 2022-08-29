using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDto>> GetProdutosPrecos()
        {
            //lista de todos os produtos por preco
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();

            //mapeando as informações através da entidade Produto para ProdutoDto
            var produtosDto = _mapper.Map<List<ProdutoDto>>(produtos);

            return produtosDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDto>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters).ToList();
            var produtosDto = _mapper.Map<List<ProdutoDto>>(produtos);

            return produtosDto;
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDto> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return produtoDto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDto produtoDto)
        {
            //transformando um Dto para entidade
            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            //transformando entidade para Dto
            var produtoDtoConvertido = _mapper.Map<ProdutoDto>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDtoConvertido);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDto produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDto> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return produtoDto;
        }
    }
}
