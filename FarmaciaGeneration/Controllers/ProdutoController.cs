﻿using FarmaciaGeneration.Model;
using FarmaciaGeneration.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FarmaciaGeneration.Controllers
{
    [Authorize]
    [ApiController]
    [Route("~/produtos")]
    public class ProdutoController : ControllerBase
    {

        private readonly IProdutoService _produtoService;
        private readonly IValidator<Produto> _produtoValidator;

        public ProdutoController(IProdutoService produtoService, IValidator<Produto> produtoValidator)
        {
            _produtoService = produtoService;
            _produtoValidator = produtoValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _produtoService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _produtoService.GetById(id);
            if (Resposta is not null)
            {
                return Ok(Resposta);
            }
            else
            {
                return NotFound();
            }

        }


        [HttpGet("nome/{nome}")]
        public async Task<ActionResult> GetByNome(string nome)
        {
            return Ok(await _produtoService.GetByNome(nome));
        }

        [HttpGet("intervaloDePreco")]
        public async Task<ActionResult> GetByIntervaloDePreco(decimal precoInicial, decimal precoFinal)
        {
            return Ok(await _produtoService.GetByIntervaloDePreco(precoInicial, precoFinal));
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult> Create([FromBody] Produto produto)
        {
            var ValidarProduto = await _produtoValidator.ValidateAsync(produto);
            if (!ValidarProduto.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ValidarProduto);
            }
            var Resposta = await _produtoService.Create(produto);

            if (Resposta is null)
            {
                return BadRequest("Produto não cadastrado!");
            }

            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }


        [HttpPost("cadastrarLista")]
        public async Task<ActionResult> CreateList([FromBody] ICollection<Produto> produtos)
        {
            var itens = "{\"Produtos\" : [";
            foreach (var produto in produtos)
            {
                var ValidarProduto = await _produtoValidator.ValidateAsync(produto);
                if (!ValidarProduto.IsValid)
                {
                    
                    itens += "{\" O Produto " + produto.Nome + " não é válido! Erro \" : \"" + ValidarProduto + "\"},";
                    continue;
                }
                var Resposta = await _produtoService.Create(produto);

                if (Resposta is null)
                {
                    
                    itens += "{\" A categoria do Produto " + produto.Nome + " não foi encontrada! Erro \" : \"" + ValidarProduto + "\"},";
                    continue;

                }

                
                itens += 
                     "{\" Id \" : \"" + produto.Id + "\","
                    + "\" Nome \" : \"" + produto.Nome + "\","
                    + "\" Descrição \" : \"" + produto.Descricao + "\","
                    + "\" Preço \" : \"" + produto.Preco + "\","
                    + "\" Imagem do Produto \" : \"" + produto.Foto + "\","
                    + "\" Categoria \" : \"" + produto.Categoria.Tipo + "\","
                    + "\" Usuário que Cadastrou \" : \"" + produto.Usuario.Nome + "\"},";
            }
            itens += "]}";
            JObject json = JObject.Parse(itens);
            return Ok(json);
        }



        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Produto produto)
        {
            if (produto.Id == 0)
            {
                return BadRequest("O Id do produto é inválido");
            }

            var ValidarProduto = await _produtoValidator.ValidateAsync(produto);
            if (!ValidarProduto.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ValidarProduto);
            }

            var Resposta = await _produtoService.Update(produto);

            if (Resposta is null)
            {
                return NotFound("Produto não encontrado(s)!");
            }

            return Ok(Resposta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {

            var BuscaProduto = await _produtoService.GetById(id);
            if (BuscaProduto is null)
            {
                return NotFound("O Produto não foi encontrado!");
            }

            await _produtoService.Delete(BuscaProduto);
            return NoContent();

        }

    }
}
