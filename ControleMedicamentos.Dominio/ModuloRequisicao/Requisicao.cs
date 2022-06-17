using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloFuncionario;

using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class Requisicao : EntidadeBase
    {

        public Requisicao()
        {
        }

        public Requisicao(Medicamento medicamento, Paciente paciente, int qtdMedicamento, DateTime data, Funcionario funcionario)
        {
            Medicamento = medicamento;
            Paciente = paciente;
            QtdMedicamento = qtdMedicamento;
            Data = data;
            Funcionario = funcionario;
        }

        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public int QtdMedicamento { get; set; }
        public DateTime Data { get; set; }
        public Funcionario Funcionario { get; set; }

        public override string ToString()
        {
            return $"{id}{" - "}{Paciente.Nome}{" - "}{Medicamento.Nome}";
        }
    }
}
