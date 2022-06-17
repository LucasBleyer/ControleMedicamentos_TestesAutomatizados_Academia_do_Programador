using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class ValidadorMedicamentoTest
    {


        private Medicamento medicamento;
        private ValidadorMedicamento validadorMedicamento;

        public ValidadorMedicamentoTest()
        {
            Db.ExecutarSql(@"DELETE FROM TBREQUISICAO;
                DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)
                DELETE FROM TBMEDICAMENTO;
                DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)
                DELETE FROM TBFORNECEDOR;
                DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");
        }

        private static Fornecedor GeraFornecedor()
        {
            Fornecedor fornecedor = new Fornecedor();
            fornecedor.Nome = "Fornecedor";
            fornecedor.Telefone = "49765432178";
            fornecedor.Email = "fornecedor@gmail.com";
            fornecedor.Cidade = "Lages";
            fornecedor.Estado = "SC";
            return fornecedor;
        }

        [TestMethod]
        public void Nome_nao_deve_ser_nulo()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = null;
            medicamento.Descricao = "Remédio para dor de cabeça";
            medicamento.Lote = "13";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;

            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = fornecedor;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }



        [TestMethod]
        public void Nome_nao_deve_ser_vazio()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "";
            medicamento.Descricao = "Remédio para dor de cabeça";
            medicamento.Lote = "13";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;


            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = fornecedor;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Nome_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "A";
            medicamento.Descricao = "Remédio para dor de cabeça";
            medicamento.Lote = "13";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;


            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = fornecedor;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Descricao_nao_deve_ser_nulo()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "Neosaldina";
            medicamento.Descricao = null;
            medicamento.Lote = "13";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;


            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = fornecedor;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Descricao_nao_deve_ser_vazio()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "Neosaldina";
            medicamento.Descricao = "";
            medicamento.Lote = "13";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;


            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = fornecedor;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Descricao_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "Neosaldina";
            medicamento.Descricao = "A";
            medicamento.Lote = "13";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;


            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = fornecedor;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Lote_nao_deve_ser_nulo()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "Neosaldina";
            medicamento.Descricao = "Remédio para dor de cabeça";
            medicamento.Lote = null;
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;


            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = fornecedor;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Lote_nao_deve_ser_vazio_e_deve_ter_no_minimo_1_caractere()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "Neosaldina";
            medicamento.Descricao = "Remédio para dor de cabeça";
            medicamento.Lote = "";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 100;


            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = fornecedor;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Fornecedor_nao_deve_ser_nulo()
        {
            //arrange
            Medicamento medicamento = new Medicamento();
            medicamento.Nome = "Neosaldina";
            medicamento.Descricao = "Remédio para dor de cabeça";
            medicamento.Lote = "10";
            medicamento.Validade = DateTime.Now;
            medicamento.QuantidadeDisponivel = 10;


            Fornecedor fornecedor = GeraFornecedor();

            medicamento.Fornecedor = null;

            validadorMedicamento = new ValidadorMedicamento();

            //action
            var resutadoValidacao = validadorMedicamento.Validate(medicamento);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }
    }
}
