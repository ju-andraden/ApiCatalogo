using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoXUnitTests
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork repository;

        public static DbContextOptions<AppDbContext>? dbContextOptions { get; }

        public static string connectionString =
            "Server=localhost;DataBase=ApiCatalogoDB;Uid=root;Pwd=123456ja*";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString))
                .Options;
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            repository = new UnitOfWork(context);
        }

        [Fact]
        public async Task GetCategoriasReturnOkResult()
        {
            //Configurar
            var controller = new CategoriasController(repository, mapper);

            //Executar
            var data = await controller.GetCategoriasProdutos();

            //Validar
            //Assert.IsType<List<IEnumerable<CategoriaDto>>>(data.Value);
            Assert.IsType<List<CategoriaDto>>(data.Value);
        }

        [Fact]
        public async Task GetCategoriasReturnBadRequest()
        {
            //configurar
            var controller = new CategoriasController(repository, mapper);

            //executar
            var data = await controller.GetCategoriasProdutos();

            //validar
            Assert.IsType<BadRequestResult>(data.Result);
        }

        [Fact]
        public async Task GetCategoriasReturnMatchResult()
        {
            var controller = new CategoriasController(repository, mapper);

            var data = await controller.GetCategoriasProdutos();

            Assert.IsType<List<CategoriaDto>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<CategoriaDto>>().Subject;

            Assert.Equal("Bebidas", cat[0].Nome);
            Assert.Equal("bebidas.jpg", cat[0].ImagemUrl);
        }

        [Fact]
        public async Task GetCategoriasByIdReturnOkResult()
        {
            var controller = new CategoriasController(repository, mapper);
            var catId = 2;

            var data = await controller.Get(catId);

            Assert.IsType<CategoriaDto>(data.Value);
        }

        [Fact]
        public async Task GetCategoriaByIdReturnNotFoundResult()
        {
            var controller = new CategoriasController(repository, mapper);
            var catId = 99;

            var data = await controller.Get(catId);

            Assert.IsType<NotFoundObjectResult>(data.Result);
        }

        [Fact]
        public async Task PostCategoriaAddValidDataReturnCreatedResult()
        {
            var controller = new CategoriasController(repository, mapper);
            var cat = new CategoriaDto() { Nome = "Teste unitário", ImagemUrl = "testeunitario.jpg" };

            var data = await controller.Post(cat);

            Assert.IsType<CreatedAtRouteResult>(data);
        }

        [Fact]
        public async Task PutCategoriaUpdateValidDataReturnOkResult()
        {
            var controller = new CategoriasController(repository, mapper);
            var catId = 2;

            //Executar
            var existingPost = await controller.Get(catId);
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDto>().Subject;
            
            var catDto = new CategoriaDto();
            catDto.CategoriaId = catId;
            catDto.Nome = "Categoria atualizada - Teste 3";
            catDto.ImagemUrl = result.ImagemUrl;

            var updatedData = await controller.Put(catId, catDto);

            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async Task DeleteCategoriaReturnOkResult()
        {
            var controller = new CategoriasController(repository, mapper);
            var catId = 9;

            //executar
            var data = await controller.Delete(catId);

            Assert.IsType<CategoriaDto>(data.Value);
        }
    }
}
