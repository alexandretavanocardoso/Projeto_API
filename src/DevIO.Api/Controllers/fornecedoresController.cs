using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class fornecedoresController : ControllerBase
    {
        private readonly IFornecedorService _fornecedorService;

        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public fornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IMapper mapper,
                                      IFornecedorService fornecedorService)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
            // usando AutoMapper para mapear, mapeamento e (font)
            var fornecedor = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return Ok(fornecedor);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            // usando AutoMapper para mapear, mapeamento e (font)
            var fornecedor = await ObterFornecedorProdutosEndereco(id);

            if(fornecedor == null)
            {
                // Erro 404  
                return NotFound();
            }

            return fornecedor;
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            var result = await _fornecedorService.Adicionar(fornecedor);

            if (!result) return BadRequest();

            return Ok(fornecedor);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            var result = await _fornecedorService.Atualizar(fornecedor);

            if (!result) return BadRequest();

            return Ok(fornecedor);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id) {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null) return NotFound();

            var resultado = await _fornecedorService.Remover(id);

            if (!resultado) return BadRequest();

            return Ok(fornecedor);
        }

        public async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid Id) { 
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(Id));
        }

        public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid Id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(Id));
        }
    }
}
