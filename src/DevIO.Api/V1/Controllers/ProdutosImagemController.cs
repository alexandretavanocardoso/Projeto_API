using AutoMapper;
using DevIO.Api.Controller;
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
    [Route("api/v1/produtosImagem")]
    [ApiController]
    public class ProdutosImagemController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosImagemController(INotificador notificador,
                                  IProdutoRepository produtoRepository,
                                  IProdutoService produtoService,
                                  IMapper mapper,
                                  IUser user) : base(notificador, user)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpPost("adicionar")]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoImagemViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(produtoViewModel);

            // Nome da imagem para nao repetir
            var imgPrefixo = Guid.NewGuid() + "_";

            if (!await UploadImagemIformFile(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return CustomResponse(ModelState);
            }

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        [RequestSizeLimit(40000000)] // Coloca um limite nas imagens
        [HttpPost("adicionarIFormFile")]
        //[DisableRequestSizeLimit] // Disabilita o limite de Request sem abrir brecha
        public ActionResult AdicionarIFormFile(IFormFile file)
        {
            return Ok(file);
        }

        private async Task<bool> UploadImagemIformFile(IFormFile arquivo, string imgPrefixo)
        {
            // Verifica se meu aqruivo é null ou vazio
            if (arquivo == null || arquivo.Length == 0)
            {
                //ModelState.AddModelError(string.Empty, "Forneça uma imagem !");
                NotificarErro("Forneça uma imagem !");
                return false;
            }

            // Pega a combinação(Meu Diretório onde aplicação esta rodando, "caminho" , nomeImagem)
            // Salvando Imagem no proprio projeto
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            // Verifica se existe essa imagem
            if (System.IO.File.Exists(path))
            {
                //ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome!");
                NotificarErro("Já existe um arquivo com esse nome!");
                return false;
            }

            // stream(manda a imagem em "pedaços") para copiar no servidor(maquina), FileMode.Create - Cria o arquivo
            using (var stream = new FileStream(path, FileMode.Create))
            {
                // Copia no servidor(maquina)
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
