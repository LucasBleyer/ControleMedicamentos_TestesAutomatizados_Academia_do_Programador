using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    public class RepositorioFuncionarioEmBancoDados
    {
        Notificador notificador = new Notificador();

        private const string enderecoBanco =
            "Data Source=(LocalDB)\\MSSQLLocalDB;" +
            "Initial Catalog=ControleMedicamentosDb;" +
            "Integrated Security=True;" +
            "Pooling=False";

        private const string sqlInserir =
            @"INSERT INTO [TBFUNCIONARIO]
                (
                    [NOME],       
                    [LOGIN], 
                    [SENHA] 
                )
            VALUES
                (
                    @NOME,
                    @LOGIN,
                    @SENHA
                ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @" UPDATE [TBFUNCIONARIO]
                    SET 
                        [NOME] = @NOME, 
                        [LOGIN] = @LOGIN, 
                        [SENHA] = @SENHA

                    WHERE [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBREQUISICAO]
                WHERE [FUNCIONARIO_ID] = @ID

                DELETE FROM [TBFUNCIONARIO] 
                    WHERE [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                [ID],       
                [NOME],
                [LOGIN],
                [SENHA]
            FROM
                [TBFUNCIONARIO]";

        private const string sqlSelecionarPorNumero =
            @"SELECT 
                [ID],       
                [NOME],
                [LOGIN],
                [SENHA]
            FROM
                [TBFUNCIONARIO]
            WHERE 
                [ID] = @ID";


        public ValidationResult Inserir(Funcionario novoRegistro)
        {
            var validador = new ValidadorFuncionario();

            var resultadoValidacao = validador.Validate(novoRegistro);

            if (resultadoValidacao.IsValid == false)
            {
                notificador.ApresentarMensagem(resultadoValidacao.ToString(), TipoMensagem.Atencao);
                return resultadoValidacao;
            }

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosFuncionario(novoRegistro, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoRegistro.id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Funcionario registro)
        {
            var validador = new ValidadorFuncionario();

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            var x = SelecionarPorNumero(registro.id);

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosFuncionario(registro, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Funcionario registro)
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

        public Funcionario SelecionarPorNumero(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorFuncionario = comandoSelecao.ExecuteReader();

            Funcionario funcionario = null;
            if (leitorFuncionario.Read())
                funcionario = ConverterParaFuncionario(leitorFuncionario);

            conexaoComBanco.Close();

            return funcionario;
        }

        public List<Funcionario> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorFuncionario = comandoSelecao.ExecuteReader();

            List<Funcionario> funcionarios = new List<Funcionario>();

            while (leitorFuncionario.Read())
            {
                Funcionario funcionario = ConverterParaFuncionario(leitorFuncionario);

                funcionarios.Add(funcionario);
            }

            conexaoComBanco.Close();

            return funcionarios;
        }

        #region Métodos privados

        private void ConfigurarParametrosFuncionario(Funcionario funcionario, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", funcionario.id);
            comando.Parameters.AddWithValue("NOME", funcionario.Nome);
            comando.Parameters.AddWithValue("LOGIN", funcionario.Login);
            comando.Parameters.AddWithValue("SENHA", funcionario.Senha);
        }

        private Funcionario ConverterParaFuncionario(SqlDataReader leitorFuncionario)
        {
            var id = Convert.ToInt32(leitorFuncionario["ID"]);
            var nome = Convert.ToString(leitorFuncionario["NOME"]);
            var login = Convert.ToString(leitorFuncionario["LOGIN"]);
            var senha = Convert.ToString(leitorFuncionario["SENHA"]);

            Funcionario funcionario = new Funcionario();
            funcionario.id = id;
            funcionario.Nome = nome;
            funcionario.Login = login;
            funcionario.Senha = senha;

            return funcionario;
        }

        #endregion

    }
}
