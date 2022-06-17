using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class Medicamento : EntidadeBase
    {        
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public int QuantidadeDisponivel { get; set; }

        public List<Requisicao> Requisicoes { get; set; }

        public Fornecedor Fornecedor{ get; set; }

        public int QuantidadeRequisicoes { get { return Requisicoes.Count; } }

        public Medicamento(string nome, string descricao, string lote, DateTime validade, int quantidade, Fornecedor fornecedor)
        {   
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            Validade = validade;
            QuantidadeDisponivel = quantidade;
            Fornecedor = fornecedor;
        }

        public Medicamento()
        {
        }

        public override string ToString()
        {
            return $"{id}{" - "}{Nome}";
        }
    }
}
