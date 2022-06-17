using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {
        private Fornecedor fornecedor;
        private RepositorioFornecedorEmBancoDados repositorio;

        public RepositorioFornecedorEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");

            fornecedor = new Fornecedor("Lages Remedios", "49367312111", "lagesremedios@gmail.com", "Lages", "SC");
            repositorio = new RepositorioFornecedorEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_fornecedor()
        {
            //action
            repositorio.Inserir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorNumero(fornecedor.id);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_fornecedor()
        {
            //arrange                      
            repositorio.Inserir(fornecedor);

            //action
            fornecedor.Nome = "Santa Catarina Remedios";
            fornecedor.Telefone = "49736283991";
            fornecedor.Email = "scremedios@gmail.com";
            fornecedor.Cidade = "Florianopolis";
            fornecedor.Estado = "SC";

            repositorio.Editar(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorNumero(fornecedor.id);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_fornecedor()
        {
            //arrange           
            repositorio.Inserir(fornecedor);

            //action           
            repositorio.Excluir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorNumero(fornecedor.id);
            Assert.IsNull(fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_fornecedor()
        {
            //arrange          
            repositorio.Inserir(fornecedor);

            //action
            var fornecedorEncontrado = repositorio.SelecionarPorNumero(fornecedor.id);

            //assert
            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_os_fornecedores()
        {
            //arrange
            var f0 = new Fornecedor("Floripa Remedios", "49726373899", "floripa@gmail.com", "Florianopolis", "SC");
            var f1 = new Fornecedor("Campeche Remedios", "49877393799", "campeche@gmail.com", "Florianopolis", "SC");
            var f2 = new Fornecedor("Camboriu Remedios", "49211723422", "camboriu@gmail.com" , "Balneario Camboriu", "SC");

            var repositorio = new RepositorioFornecedorEmBancoDados();
            repositorio.Inserir(f0);
            repositorio.Inserir(f1);
            repositorio.Inserir(f2);

            //action
            var fornecedores = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, fornecedores.Count);

            Assert.AreEqual(f0.Nome, fornecedores[0].Nome);
            Assert.AreEqual(f1.Nome, fornecedores[1].Nome);
            Assert.AreEqual(f2.Nome, fornecedores[2].Nome);
        }
    }
}
