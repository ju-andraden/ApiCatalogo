using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategoriasProdutos()
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasProdutos();

            var categoriasDto = _mapper.Map<List<CategoriaDto>>(categorias);

            return categoriasDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.Get().ToListAsync();

            var categoriasDto = _mapper.Map<List<CategoriaDto>>(categorias);

            return categoriasDto;
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDto>> Get(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return categoriaDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _uof.CategoriaRepository.Add(categoria);
            await _uof.Commit();

            var categoriaDtoConvertido = _mapper.Map<CategoriaDto>(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoriaDtoConvertido);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDto categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest("Dados inválidos.");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _uof.CategoriaRepository.Update(categoria);
            await _uof.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDto>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            _uof.CategoriaRepository.Delete(categoria);
            await _uof.Commit();

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return categoriaDto;
        }
    }
}
