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

        public Fornecedor Fornecedor{ get; set; }

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

        public override bool Equals(object obj)
        {
            return obj is Medicamento medicamento &&
                   id == medicamento.id &&
                   Nome == medicamento.Nome &&
                   Descricao == medicamento.Descricao &&
                   Lote == medicamento.Lote &&
                   Validade == medicamento.Validade &&
                   QuantidadeDisponivel == medicamento.QuantidadeDisponivel &&
                   EqualityComparer<Fornecedor>.Default.Equals(Fornecedor, medicamento.Fornecedor);
        }
    }
}
