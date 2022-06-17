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

        public override bool Equals(object obj)
        {
            return obj is Requisicao requisicao &&
                   id == requisicao.id &&
                   EqualityComparer<Medicamento>.Default.Equals(Medicamento, requisicao.Medicamento) &&
                   EqualityComparer<Paciente>.Default.Equals(Paciente, requisicao.Paciente) &&
                   QtdMedicamento == requisicao.QtdMedicamento &&
                   Data == requisicao.Data &&
                   EqualityComparer<Funcionario>.Default.Equals(Funcionario, requisicao.Funcionario);
        }

        public override string ToString()
        {
            return $"{id}{" - "}{Paciente.Nome}{" - "}{Medicamento.Nome}";
        }


    }
}
