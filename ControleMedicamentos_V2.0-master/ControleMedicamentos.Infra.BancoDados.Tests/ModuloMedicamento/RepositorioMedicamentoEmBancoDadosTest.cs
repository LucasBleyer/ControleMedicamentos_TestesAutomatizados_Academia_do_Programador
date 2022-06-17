using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {
        private Medicamento medicamento;
        private Fornecedor fornecedor;
        private RepositorioMedicamentoEmBancoDados repositorioMedicamento;
        private RepositorioFornecedorEmBancoDados repositorioFornecedor;
        private TelaCadastroMedicamento tela;
        private Notificador notificador;

        public RepositorioMedicamentoEmBancoDadosTest()
        {

            Db.ExecutarSql(@"DELETE FROM TBREQUISICAO;
                DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)
                DELETE FROM TBMEDICAMENTO;
                DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)
                DELETE FROM TBFORNECEDOR;
                DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");


            fornecedor = new Fornecedor("Fornecedor", "11111111111", "fornecedor@gmail.com", "Lages", "SC");
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
            repositorioFornecedor.Inserir(fornecedor);
            medicamento = new Medicamento("Medicação", "Descrição", "12", DateTime.Now.Date, 10, fornecedor);
            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            notificador = new Notificador();
            tela = new TelaCadastroMedicamento(repositorioMedicamento, notificador);
        }


        [TestMethod]
        public void Deve_inserir_novo_medicamento()
        {
            //arrange
            repositorioMedicamento.Inserir(medicamento);

            //action
            Medicamento medicamentoEncontrado = repositorioMedicamento.SelecionarPorNumero(medicamento.id);

            //assert
            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_medicamento()
        {
            //arrange                      
            repositorioMedicamento.Inserir(medicamento);

            //action
            medicamento.Nome = "Outro nome";
            repositorioMedicamento.Editar(medicamento);

            //assert
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorNumero(medicamento.id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_medicamento()
        {
            //arrange           
            repositorioMedicamento.Inserir(medicamento);

            //action           
            repositorioMedicamento.Excluir(medicamento);

            //assert
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorNumero(medicamento.id);
            Assert.IsNull(medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_medicamento()
        {
            //arrange          
            repositorioMedicamento.Inserir(medicamento);

            //action
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorNumero(medicamento.id);

            //assert
            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_os_medicamentos()
        {
            //arrange
            var m0 = new Medicamento("Medicamento1", "Descricao1", "1", DateTime.Now.Date, 10, fornecedor);
            var m1 = new Medicamento("Medicamento2", "Descricao2", "2", DateTime.Now.Date, 20, fornecedor);
            var m2 = new Medicamento("Medicamento3", "Descricao3", "3", DateTime.Now.Date, 30, fornecedor);

            repositorioMedicamento.Inserir(m0);
            repositorioMedicamento.Inserir(m1);
            repositorioMedicamento.Inserir(m2);

            //action
            var medicamentos = repositorioMedicamento.SelecionarTodos();

            //assert

            Assert.AreEqual(3, medicamentos.Count);

            Assert.AreEqual(m0.Nome, medicamentos[0].Nome);
            Assert.AreEqual(m1.Nome, medicamentos[1].Nome);
            Assert.AreEqual(m2.Nome, medicamentos[2].Nome);
        }

        [TestMethod]
        public void Deve_selecionar_medicamentos_em_falta()
        {
            //arrange
            var m0 = new Medicamento("Medicamento1", "Descricao1", "1", DateTime.Now.Date, 10, fornecedor);
            var m1 = new Medicamento("Medicamento2", "Descricao2", "2", DateTime.Now.Date, 20, fornecedor);
            var m2 = new Medicamento("Medicamento3", "Descricao3", "3", DateTime.Now.Date, 30, fornecedor);

            repositorioMedicamento.Inserir(m0);
            repositorioMedicamento.Inserir(m1);
            repositorioMedicamento.Inserir(m2);

            //action
            var medicamentos = tela.MostrarMedicamentosEmFalta();

            //assert

            Assert.AreEqual(1, medicamentos.Count);

        }
    }
}
