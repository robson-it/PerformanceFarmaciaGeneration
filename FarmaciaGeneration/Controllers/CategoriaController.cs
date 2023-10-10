using FarmaciaGeneration.Model;
using FarmaciaGeneration.Service;
using FarmaciaGeneration.Service.Implements;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FarmaciaGeneration.Controllers
{
    [ApiController]
    [Route("~/categorias")]
    public class CategoriaController : ControllerBase
    {

        private readonly ICategoriaService _categoriaService;
        private readonly IValidator<Categoria> _categoriaValidator;

        public CategoriaController(ICategoriaService categoriaService, IValidator<Categoria> categoriaValidator)
        {
            _categoriaService = categoriaService;
            _categoriaValidator = categoriaValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _categoriaService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _categoriaService.GetById(id);
            if (Resposta is not null)
            {
                return Ok(Resposta);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult> GetByTipo(string tipo)
        {
            return Ok(await _categoriaService.GetByTipo(tipo));
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult> Create([FromBody] Categoria categoria)
        {
            var ValidarCategoria = await _categoriaValidator.ValidateAsync(categoria);
            if (!ValidarCategoria.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ValidarCategoria);
            }
            await _categoriaService.Create(categoria);
            return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Categoria categoria)
        {
            if (categoria.Id == 0)
            {
                return BadRequest("O Id da categoria é inválido");
            }

            var ValidarCategoria = await _categoriaValidator.ValidateAsync(categoria);
            if (!ValidarCategoria.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ValidarCategoria);
            }

            var Resposta = await _categoriaService.Update(categoria);

            if (Resposta is null)
            {
                return NotFound("Categoria não encontrada!");
            }

            return Ok(Resposta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {

            var BuscaCategoria = await _categoriaService.GetById(id);
            if (BuscaCategoria is null)
            {
                return NotFound("A categoria não foi encontrada!");
            }

            await _categoriaService.Delete(BuscaCategoria);
            return NoContent();

        }


    }
}
