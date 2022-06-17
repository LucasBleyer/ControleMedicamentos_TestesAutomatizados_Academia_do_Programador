using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        private Funcionario funcionario;
        private RepositorioFuncionarioEmBancoDados repositorio;

        public RepositorioFuncionarioEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");

            funcionario = new Funcionario("Lucas Bleyer", "lucas_bleyer", "123456789");
            repositorio = new RepositorioFuncionarioEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_funcionario()
        {
            //action
            repositorio.Inserir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorNumero(funcionario.id);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario, funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_funcionario()
        {
            //arrange                      
            repositorio.Inserir(funcionario);

            //action
            funcionario.Nome = "Lucas Oliveira Bleyer";
            funcionario.Login = "lucas_bleyer";
            funcionario.Senha = "8888";
            repositorio.Editar(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorNumero(funcionario.id);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario, funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_funcionario()
        {
            //arrange           
            repositorio.Inserir(funcionario);

            //action           
            repositorio.Excluir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorNumero(funcionario.id);
            Assert.IsNull(funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_funcionario()
        {
            //arrange          
            repositorio.Inserir(funcionario);

            //action
            var funcionarioEncontrado = repositorio.SelecionarPorNumero(funcionario.id);

            //assert
            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario, funcionarioEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_os_funcionarios()
        {
            //arrange
            var f0 = new Funcionario("Tales Reig", "tales_r" , "1111111111");
            var f1 = new Funcionario("Pedro Nunes", "pedrinho_n", "222222222");
            var f2 = new Funcionario("Ane Luisy", "anee_luisy", "333333333");

            var repositorio = new RepositorioFuncionarioEmBancoDados();
            repositorio.Inserir(f0);
            repositorio.Inserir(f1);
            repositorio.Inserir(f2);

            //action
            var funcionarios = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, funcionarios.Count);

            Assert.AreEqual(f0.Nome, funcionarios[0].Nome);
            Assert.AreEqual(f1.Nome, funcionarios[1].Nome);
            Assert.AreEqual(f2.Nome, funcionarios[2].Nome);
        }
    }
}
