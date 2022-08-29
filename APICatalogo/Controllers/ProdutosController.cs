using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();

            var produtosDto = _mapper.Map<List<ProdutoDto>>(produtos);

            return produtosDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> Get()
        {
            var produtos = await _uof.ProdutoRepository.Get().ToListAsync();

            var produtosDto = _mapper.Map<List<ProdutoDto>>(produtos);

            return produtosDto;
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDto>> Get(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return produtoDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDto produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Add(produto);
            await _uof.Commit();

            var produtoDtoConvertido = _mapper.Map<ProdutoDto>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDtoConvertido);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDto produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDto>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDto>(produto);

            return produtoDto;
        }
    }
}
