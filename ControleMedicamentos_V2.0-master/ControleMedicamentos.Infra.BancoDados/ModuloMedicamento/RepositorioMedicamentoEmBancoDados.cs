using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados 
    {
        RepositorioFornecedorEmBancoDados repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
        Notificador notificador = new Notificador();

        private const string enderecoBanco =
            "Data Source=(LocalDB)\\MSSQLLocalDB;" +
            "Initial Catalog=ControleMedicamentosDb;" +
            "Integrated Security=True;" +
            "Pooling=False";

        #region Sql Queries

        private const string sqlInserir =
            @"INSERT INTO [TBMEDICAMENTO]
                (
                    [NOME],       
                    [DESCRICAO], 
                    [LOTE],
                    [VALIDADE],                    
                    [QUANTIDADEDISPONIVEL],                                                           
                    [FORNECEDOR_ID]       
                )
            VALUES
                (
                    @NOME,
                    @DESCRICAO,
                    @LOTE,
                    @VALIDADE,
                    @QUANTIDADEDISPONIVEL,
                    @FORNECEDOR_ID
                ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @" UPDATE [TBMEDICAMENTO]
                    SET 
                        [NOME] = @NOME, 
                        [DESCRICAO] = @DESCRICAO, 
                        [LOTE] = @LOTE,
                        [VALIDADE] = @VALIDADE, 
                        [QUANTIDADEDISPONIVEL] = @QUANTIDADEDISPONIVEL,
                        [FORNECEDOR_ID] = @FORNECEDOR_ID

                    WHERE [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBREQUISICAO]
                WHERE [MEDICAMENTO_ID] = @ID

                DELETE FROM [TBMEDICAMENTO] 
                WHERE [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                MEDICAMENTO.[ID],       
                MEDICAMENTO.[NOME],
                MEDICAMENTO.[DESCRICAO],
                MEDICAMENTO.[LOTE],             
                MEDICAMENTO.[VALIDADE],                    
                MEDICAMENTO.[QUANTIDADEDISPONIVEL],                                
                MEDICAMENTO.[FORNECEDOR_ID],
                FORNECEDOR.[NOME],              
                FORNECEDOR.[TELEFONE],                    
                FORNECEDOR.[EMAIL], 
                FORNECEDOR.[CIDADE],
                FORNECEDOR.[ESTADO]
            FROM
                [TBMEDICAMENTO] AS MEDICAMENTO LEFT JOIN 
                [TBFORNECEDOR] AS FORNECEDOR
            ON
                FORNECEDOR.ID = MEDICAMENTO.FORNECEDOR_ID";

        private const string sqlSelecionarPorNumero =
            @"SELECT 
                MEDICAMENTO.[ID],       
                MEDICAMENTO.[NOME],
                MEDICAMENTO.[DESCRICAO],
                MEDICAMENTO.[LOTE],             
                MEDICAMENTO.[VALIDADE],                    
                MEDICAMENTO.[QUANTIDADEDISPONIVEL],                                
                MEDICAMENTO.[FORNECEDOR_ID],
                FORNECEDOR.[NOME],              
                FORNECEDOR.[TELEFONE],                    
                FORNECEDOR.[EMAIL], 
                FORNECEDOR.[CIDADE],
                FORNECEDOR.[ESTADO]
            FROM
                [TBMEDICAMENTO] AS MEDICAMENTO LEFT JOIN 
                [TBFORNECEDOR] AS FORNECEDOR
            ON
                FORNECEDOR.ID = MEDICAMENTO.FORNECEDOR_ID
            WHERE 
                MEDICAMENTO.[ID] = @ID";

        #endregion

        public ValidationResult Inserir(Medicamento novoRegistro)
        {
            var validador = new ValidadorMedicamento();

            var resultadoValidacao = validador.Validate(novoRegistro);

            if (resultadoValidacao.IsValid == false)
            {
                notificador.ApresentarMensagem(resultadoValidacao.ToString(), TipoMensagem.Atencao);
                return resultadoValidacao;
            }

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosMedicamento(novoRegistro, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoRegistro.id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Medicamento registro)
        {
            var validador = new ValidadorMedicamento();

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            var x = SelecionarPorNumero(registro.id);

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosMedicamento(registro, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Medicamento registro)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", registro.id);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public Medicamento SelecionarPorNumero(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            Medicamento medicamento = null;
            if (leitorMedicamento.Read())
                medicamento = ConverterParaMedicamento(leitorMedicamento);

            conexaoComBanco.Close();

            return medicamento;
        }

        public List<Medicamento> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            List<Medicamento> medicamentos = new List<Medicamento>();

            while (leitorMedicamento.Read())
            {
                Medicamento medicamento = ConverterParaMedicamento(leitorMedicamento);

                medicamentos.Add(medicamento);
            }

            conexaoComBanco.Close();

            return medicamentos;
        }

        #region Métodos privados

        private void ConfigurarParametrosMedicamento(Medicamento medicamento, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", medicamento.id);
            comando.Parameters.AddWithValue("NOME", medicamento.Nome);
            comando.Parameters.AddWithValue("DESCRICAO", medicamento.Descricao);
            comando.Parameters.AddWithValue("LOTE", medicamento.Lote);
            comando.Parameters.AddWithValue("VALIDADE", medicamento.Validade);
            comando.Parameters.AddWithValue("QUANTIDADEDISPONIVEL", medicamento.QuantidadeDisponivel);

            comando.Parameters.AddWithValue("FORNECEDOR_ID", medicamento.Fornecedor.id);
        }

        private Medicamento ConverterParaMedicamento(SqlDataReader leitorMedicamento)
        {
            var id = Convert.ToInt32(leitorMedicamento["ID"]);
            var nome = Convert.ToString(leitorMedicamento["NOME"]);
            var descricao = Convert.ToString(leitorMedicamento["DESCRICAO"]);
            var lote = Convert.ToString(leitorMedicamento["LOTE"]);
            var validade = Convert.ToDateTime(leitorMedicamento["VALIDADE"]);
            var quantidadeDisponivel = Convert.ToInt32(leitorMedicamento["QUANTIDADEDISPONIVEL"]);

            Medicamento medicamento = new Medicamento();
            medicamento.id = id;
            medicamento.Nome = nome;
            medicamento.Descricao = descricao;
            medicamento.Lote = lote;
            medicamento.Validade = validade;
            medicamento.QuantidadeDisponivel = quantidadeDisponivel;

            var idFornecedor = Convert.ToInt32(leitorMedicamento["FORNECEDOR_ID"]);
            medicamento.Fornecedor = repositorioFornecedor.SelecionarPorNumero(idFornecedor);
            
            return medicamento;
        }

        #endregion
    }
}
