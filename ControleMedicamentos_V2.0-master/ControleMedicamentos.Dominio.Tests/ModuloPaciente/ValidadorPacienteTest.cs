using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloPaciente
{
    [TestClass]
    public class ValidadorPacienteTest
    {


        private Paciente paciente;
        private ValidadorPaciente validadorPaciente;

        public ValidadorPacienteTest()
        {
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");
        }

        [TestMethod]
        public void Nome_nao_deve_ser_nulo()
        {
            //arrange
            paciente = new Paciente();
            paciente.Nome = null;
            paciente.CartaoSUS = "987654321";
            validadorPaciente = new ValidadorPaciente();

            //action
            var resutadoValidacao = validadorPaciente.Validate(paciente);

            //assert
            Assert.AreEqual(false , resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Nome_nao_deve_ser_vazio()
        {
            //arrange
            paciente = new Paciente();
            paciente.Nome = "";
            paciente.CartaoSUS = "987654321";
            validadorPaciente = new ValidadorPaciente();

            //action
            var resutadoValidacao = validadorPaciente.Validate(paciente);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void Nome_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            paciente = new Paciente();
            paciente.Nome = "A";
            paciente.CartaoSUS = "987654321";
            validadorPaciente = new ValidadorPaciente();

            //action
            var resutadoValidacao = validadorPaciente.Validate(paciente);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void CartaoSus_nao_deve_ser_nulo()
        {
            //arrange
            paciente = new Paciente();
            paciente.Nome = "Paula";
            paciente.CartaoSUS = null;
            validadorPaciente = new ValidadorPaciente();

            //action
            var resutadoValidacao = validadorPaciente.Validate(paciente);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void CartaoSus_nao_deve_ser_vazio()
        {
            //arrange
            paciente = new Paciente();
            paciente.Nome = "Paula";
            paciente.CartaoSUS = "";
            validadorPaciente = new ValidadorPaciente();

            //action
            var resutadoValidacao = validadorPaciente.Validate(paciente);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }

        [TestMethod]
        public void CartaoSus_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            paciente = new Paciente();
            paciente.Nome = "Paula";
            paciente.CartaoSUS = "0";
            validadorPaciente = new ValidadorPaciente();

            //action
            var resutadoValidacao = validadorPaciente.Validate(paciente);

            //assert
            Assert.AreEqual(false, resutadoValidacao.IsValid);
        }
    }
}
