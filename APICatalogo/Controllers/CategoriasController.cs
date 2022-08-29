using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<CategoriaDto>> GetCategoriasProdutos()
        {
            var categorias = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();

            var categoriasDto = _mapper.Map<List<CategoriaDto>>(categorias);

            return categoriasDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDto>> Get()
        {
            var categorias = _uof.CategoriaRepository.Get().ToList();

            var categoriasDto = _mapper.Map<List<CategoriaDto>>(categorias);

            return categoriasDto;
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDto> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return categoriaDto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            var categoriaDtoConvertido = _mapper.Map<CategoriaDto>(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoriaDtoConvertido);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CategoriaDto categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest("Dados inválidos.");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDto> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return categoriaDto;
        }
    }
}
