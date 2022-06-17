using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloFuncionario
{
    [TestClass]
    public class ValidadorFuncionarioTest
    {
        private Funcionario funcionario;
        private ValidadorFuncionario validadorFuncionario;

        public ValidadorFuncionarioTest()
        {
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");
        }

        [TestMethod]
        public void Nome_nao_deve_ser_nulo()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = null;
            funcionario.Login = "login";
            funcionario.Senha = "123456";

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Nome_nao_deve_ser_vazio()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "";
            funcionario.Login = "login";
            funcionario.Senha = "123456";

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Nome_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "A";
            funcionario.Login = "login";
            funcionario.Senha = "123456";

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Login_nao_deve_ser_nulo()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "Paula";
            funcionario.Login = null;
            funcionario.Senha = "123456";

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Login_nao_deve_ser_vazio()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "Paula";
            funcionario.Login = "";
            funcionario.Senha = "123456";

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Login_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "Paula";
            funcionario.Login = "A";
            funcionario.Senha = "123456";

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Senha_nao_deve_ser_nulo()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "Paula";
            funcionario.Login = "paula.login";
            funcionario.Senha = null;

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Senha_nao_deve_ser_vazio()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "Paula";
            funcionario.Login = "paula.login";
            funcionario.Senha = "";

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Senha_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "Paula";
            funcionario.Login = "paula.login";
            funcionario.Senha = "1";

            validadorFuncionario = new ValidadorFuncionario();

            //action
            var resutadoValidacao = validadorFuncionario.Validate(funcionario);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }




    }
}
