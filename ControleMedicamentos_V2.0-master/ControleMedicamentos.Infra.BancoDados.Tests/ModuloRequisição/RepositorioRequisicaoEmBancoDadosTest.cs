using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisição
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        private Requisicao requisicao;
        private Medicamento medicamento;
        private Funcionario funcionario;
        private Paciente paciente;
        private Fornecedor fornecedor;
        private RepositorioFornecedorEmBancoDados repositorioFornecedor;
        private RepositorioRequisicaoEmBancoDados repositorioRequisicao;
        private RepositorioMedicamentoEmBancoDados repositorioMedicamento;
        private RepositorioFuncionarioEmBancoDados repositorioFuncionario;
        private RepositorioPacienteEmBancoDados repositorioPaciente;

        public RepositorioRequisicaoEmBancoDadosTest()
        {

            Db.ExecutarSql(@"DELETE FROM TBREQUISICAO;
                  DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)
                  DELETE FROM TBMEDICAMENTO;
                  DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)
                  DELETE FROM TBFUNCIONARIO;
                  DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)
                  DELETE FROM TBPACIENTE;
                  DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");

            fornecedor = new Fornecedor("Fornecedor", "11111111111", "fornecedor@gmail.com", "Lages", "SC");
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
            repositorioFornecedor.Inserir(fornecedor);

            medicamento = new Medicamento("Neosaldina", "Descricao", "12", DateTime.Now.Date, 50, fornecedor);
            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            repositorioMedicamento.Inserir(medicamento);

            funcionario = new Funcionario("Funcionario", "Login", "12345");
            repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();
            repositorioFuncionario.Inserir(funcionario);

            paciente = new Paciente("Paciente", "123abc");
            repositorioPaciente = new RepositorioPacienteEmBancoDados();
            repositorioPaciente.Inserir(paciente);

            requisicao = new Requisicao(medicamento, paciente, 5, DateTime.Now.Date, funcionario);
            repositorioRequisicao = new RepositorioRequisicaoEmBancoDados();

        }


        [TestMethod]
        public void Deve_inserir_nova_requisicao()
        {
            //arrange
            repositorioRequisicao.Inserir(requisicao);

            //action
            Requisicao requisicaoEncontrada = repositorioRequisicao.SelecionarPorNumero(requisicao.id);

            //assert
            Assert.IsNotNull(requisicaoEncontrada);
            Assert.AreEqual(requisicao, requisicaoEncontrada);
        }

        [TestMethod]
        public void Deve_editar_informacoes_requisicao()
        {
            //arrange                      
            repositorioRequisicao.Inserir(requisicao);

            //action
            requisicao.QtdMedicamento = 3;
            repositorioRequisicao.Editar(requisicao);

            //assert
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorNumero(requisicao.id);

            Assert.IsNotNull(requisicaoEncontrada);
            Assert.AreEqual(requisicao, requisicaoEncontrada);
        }

        [TestMethod]
        public void Deve_excluir_requisicao()
        {
            //arrange           
            repositorioRequisicao.Inserir(requisicao);

            //action           
            repositorioRequisicao.Excluir(requisicao);

            //assert
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorNumero(medicamento.id);
            Assert.IsNull(requisicaoEncontrada);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_uma_requisicao()
        {
            //arrange          
            repositorioRequisicao.Inserir(requisicao);

            //action
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorNumero(requisicao.id);

            //assert
            Assert.IsNotNull(requisicaoEncontrada);
            Assert.AreEqual(requisicao, requisicaoEncontrada);
        }

        [TestMethod]
        public void Deve_selecionar_todas_as_requisicoes()
        {
            //arrange
            var r0 = new Requisicao(medicamento, paciente, 1, DateTime.Now.Date, funcionario);
            var r1 = new Requisicao(medicamento, paciente, 2, DateTime.Now.Date, funcionario);
            var r2 = new Requisicao(medicamento, paciente, 3, DateTime.Now.Date, funcionario);

            repositorioRequisicao.Inserir(r0);
            repositorioRequisicao.Inserir(r1);
            repositorioRequisicao.Inserir(r2);

            //action
            var requisicoes = repositorioRequisicao.SelecionarTodos();

            //assert

            Assert.AreEqual(3, requisicoes.Count);

            Assert.AreEqual(r0.QtdMedicamento, requisicoes[0].QtdMedicamento);
            Assert.AreEqual(r1.QtdMedicamento, requisicoes[1].QtdMedicamento);
            Assert.AreEqual(r2.QtdMedicamento, requisicoes[2].QtdMedicamento);
        }

    }
}