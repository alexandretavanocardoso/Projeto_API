using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/produtos")]
    [ApiController]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosController(INotificador notificador,
                                  IProdutoRepository produtoRepository,
                                  IProdutoService produtoService,
                                  IMapper mapper,
                                  IUser user) : base(notificador, user)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return produtoViewModel;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
        {
            var produto = await ObterProduto(id);

            if (produto == null) return NotFound();

            await _produtoRepository.Remover(id);

            return CustomResponse(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(produtoViewModel);

            // Nome da imagem para nao repetir
            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;

            if (!UploadImagem(produtoViewModel.ImagemUpload, imagemNome))
            {
                return CustomResponse();
            }

            produtoViewModel.Imagem = imagemNome;
            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        private bool UploadImagem(string arquivo, string imgNome)
        {
            // Verifica se meu aqruivo é null ou vazio
            if (string.IsNullOrEmpty(arquivo))
            {
                //ModelState.AddModelError(string.Empty, "Forneça uma imagem !");
                NotificarErro("Forneça uma imagem !");
                return false;
            }
        
            // Converte para Base64
            var imageDataByteArray = Convert.FromBase64String(arquivo);

            // Pega a combinação(Meu Diretório onde aplicação esta rodando, "caminho" , nomeImagem)
            // Salvando Imagem no proprio projeto
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

            // Verifica se existe essa imagem
            if (System.IO.File.Exists(filePath))
            {
                //ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome!");
                NotificarErro("Já existe um arquivo com esse nome!");
                return false;
            }

            // GerarArquivo(Combinação do caminho , Combinação de bytes)
            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        public async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
        }
    }
}
