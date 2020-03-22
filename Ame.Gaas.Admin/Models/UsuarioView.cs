using ClassLibrary.Gaas.Business.Usuario;
using ClassLibrary.Gaas.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ame.Gaas.Admin.Models
{
    public class UsuarioView
    {

        public string id { get; set; }
        public string nome { get; set; }
        [Required(ErrorMessage = "Email é obrigatório")]
        public string email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string senha { get; set; }
        [Required(ErrorMessage = "Repetir a senha é obrigatório")]
        public string repeteSenha { get; set; }
        public DateTime dataCadastro { get; set; }
        public bool ativo { get; set; }


        public UsuarioView() { this.dataCadastro = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()); }

        public UsuarioView(string id, string nome, string email, string senha, string repeteSenha, DateTime dataCadastro, bool ativo)
        {
            this.id = id;
            this.nome = nome;
            this.email = email;
            this.senha = senha;
            this.repeteSenha = repeteSenha;
            this.dataCadastro = new Utils().convertDatePtBr(dataCadastro);
            this.ativo = ativo;
        }

        public bool autenticaUsr(string loginEmail, string loginSenha) {


            Usuario acesso = new Usuario();

            bool resp = acesso.performaLogin(loginEmail, loginSenha);

            if (resp)
            {

                return true;
            }

            return false;
        }

    }
}