using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDados
    {
        Notificador notificador = new Notificador();

        private const string enderecoBanco =
            "Data Source=(LocalDB)\\MSSQLLocalDB;" +
            "Initial Catalog=ControleMedicamentosDb;" +
            "Integrated Security=True;" +
            "Pooling=False";

        private const string sqlInserir =
            @"INSERT INTO [TBFORNECEDOR]
                (
                    [NOME],       
                    [TELEFONE], 
                    [EMAIL],
                    [CIDADE],                    
                    [ESTADO]   
                )
            VALUES
                (
                    @NOME,
                    @TELEFONE,
                    @EMAIL,
                    @CIDADE,
                    @ESTADO
                ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @" UPDATE [TBFORNECEDOR]
                    SET 
                        [NOME] = @NOME, 
                        [TELEFONE] = @TELEFONE, 
                        [EMAIL] = @EMAIL,
                        [CIDADE] = @CIDADE, 
                        [ESTADO] = @ESTADO

                    WHERE [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBMEDICAMENTO]
                WHERE [FORNECEDOR_ID] = @ID

                DELETE FROM [TBFORNECEDOR] 
                    WHERE [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                [ID],       
                [NOME],
                [TELEFONE],
                [EMAIL],             
                [CIDADE],                    
                [ESTADO]
            FROM
                [TBFORNECEDOR]";

        private const string sqlSelecionarPorNumero =
            @"SELECT 
                [ID],       
                [NOME],
                [TELEFONE],
                [EMAIL],             
                [CIDADE],                    
                [ESTADO]
            FROM
                [TBFORNECEDOR]
            WHERE 
                [ID] = @ID";


        public ValidationResult Inserir(Fornecedor novoRegistro)
        {
            var validador = new ValidadorFornecedor();

            var resultadoValidacao = validador.Validate(novoRegistro);
             
            if (resultadoValidacao.IsValid == false)
            {
                notificador.ApresentarMensagem(resultadoValidacao.ToString(), TipoMensagem.Atencao);
                return resultadoValidacao;
            }

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosFornecedor(novoRegistro, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoRegistro.id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Fornecedor registro)
        {
            var validador = new ValidadorFornecedor();

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            var x = SelecionarPorNumero(registro.id);

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosFornecedor(registro, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Fornecedor registro)
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

        public Fornecedor SelecionarPorNumero(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            Fornecedor fornecedor = null;
            if (leitorFornecedor.Read())
                fornecedor = ConverterParaFornecedor(leitorFornecedor);

            conexaoComBanco.Close();

            return fornecedor;
        }

        public List<Fornecedor> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            List<Fornecedor> fornecedores = new List<Fornecedor>();

            while (leitorFornecedor.Read())
            {
                Fornecedor fornecedor = ConverterParaFornecedor(leitorFornecedor);

                fornecedores.Add(fornecedor);
            }

            conexaoComBanco.Close();

            return fornecedores;
        }

        #region Métodos privados

        private void ConfigurarParametrosFornecedor(Fornecedor fornecedor, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", fornecedor.id);
            comando.Parameters.AddWithValue("NOME", fornecedor.Nome);
            comando.Parameters.AddWithValue("TELEFONE", fornecedor.Telefone);
            comando.Parameters.AddWithValue("EMAIL", fornecedor.Email);
            comando.Parameters.AddWithValue("CIDADE", fornecedor.Cidade);
            comando.Parameters.AddWithValue("ESTADO ", fornecedor.Estado);
        }

        private Fornecedor ConverterParaFornecedor(SqlDataReader leitorFornecedor)
        {
            var id = Convert.ToInt32(leitorFornecedor["ID"]);
            var nome = Convert.ToString(leitorFornecedor["NOME"]);
            var telefone = Convert.ToString(leitorFornecedor["TELEFONE"]);
            var email = Convert.ToString(leitorFornecedor["EMAIL"]);
            var cidade = Convert.ToString(leitorFornecedor["CIDADE"]);
            var estado = Convert.ToString(leitorFornecedor["ESTADO"]);

            Fornecedor fornecedor = new Fornecedor();
            fornecedor.id = id;
            fornecedor.Nome = nome;
            fornecedor.Telefone = telefone;
            fornecedor.Email = email;
            fornecedor.Cidade = cidade;
            fornecedor.Estado = estado;

            return fornecedor;
        }

        #endregion

    }
}
