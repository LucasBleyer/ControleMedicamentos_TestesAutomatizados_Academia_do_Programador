using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloRequisição
{
    [TestClass]
    public class ValidadorRequisicaoTest
    {


        private Requisicao requisicao;
        private ValidadorRequisicao validadorRequisicao;

        public ValidadorRequisicaoTest()
        {
            Db.ExecutarSql(@"DELETE FROM TBREQUISICAO;
                  DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)
                  DELETE FROM TBMEDICAMENTO;
                  DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)
                  DELETE FROM TBFUNCIONARIO;
                  DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)
                  DELETE FROM TBPACIENTE;
                  DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");
        }

        private static Funcionario GeraFuncionario()
        {
            Funcionario funcionario = new Funcionario();
            funcionario.Nome = "Funcionario";
            funcionario.Login = "login";
            funcionario.Senha = "123456";

            return funcionario;
        }

        private static Medicamento GeraMedicamento()
        {
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "Neosaldina";
            medicamento.Descricao = "Remédio para dor de cabeça";
            medicamento.Lote = "13";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;

            return medicamento;
        }

        private static Paciente GeraPaciente()
        {
            Paciente paciente = new Paciente();
            paciente.Nome = "Paciente";
            paciente.CartaoSUS = "987654321";

            return paciente;
        }

        [TestMethod]
        public void Medicamento_nao_deve_ser_nulo()
        {
            //arrange
            Requisicao requisicao = new Requisicao();
            requisicao.Medicamento = null;
            requisicao.Paciente = GeraPaciente();
            requisicao.QtdMedicamento = 10;
            requisicao.Data = DateTime.Now;
            requisicao.Funcionario = GeraFuncionario();

            validadorRequisicao = new ValidadorRequisicao();

            //action
            var resutadoValidacao = validadorRequisicao.Validate(requisicao);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Paciente_nao_deve_ser_nulo()
        {
            //arrange
            Requisicao requisicao = new Requisicao();
            requisicao.Medicamento = GeraMedicamento();
            requisicao.Paciente = null;
            requisicao.QtdMedicamento = 10;
            requisicao.Data = DateTime.Now;
            requisicao.Funcionario = GeraFuncionario();

            validadorRequisicao = new ValidadorRequisicao();

            //action
            var resutadoValidacao = validadorRequisicao.Validate(requisicao);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Funcionario_nao_deve_ser_nulo()
        {
            //arrange
            Requisicao requisicao = new Requisicao();
            requisicao.Medicamento = GeraMedicamento();
            requisicao.Paciente = GeraPaciente();
            requisicao.QtdMedicamento = 10;
            requisicao.Data = DateTime.Now;
            requisicao.Funcionario = null;

            validadorRequisicao = new ValidadorRequisicao();

            //action
            var resutadoValidacao = validadorRequisicao.Validate(requisicao);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }
    }
}
