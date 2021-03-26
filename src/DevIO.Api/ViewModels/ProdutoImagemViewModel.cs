using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevIO.Api.ViewModels
{
    [ModelBinder(typeof(JsonWithFilesFormDataModelBinder), Name =  "produto")]
    public class ProdutoImagemViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string Descrição { get; set; }

        public IFormFile ImagemUpload { get; set; }

        public string Imagem { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Valor { get; set; }
        
        [ScaffoldColumn(false)] // Faz com que não represente por um dado de entrada
        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }

        [ScaffoldColumn(false)] // Faz com que não represente por um dado de entrada
        public string NomeFornecedor { get; set; }

    }
}
