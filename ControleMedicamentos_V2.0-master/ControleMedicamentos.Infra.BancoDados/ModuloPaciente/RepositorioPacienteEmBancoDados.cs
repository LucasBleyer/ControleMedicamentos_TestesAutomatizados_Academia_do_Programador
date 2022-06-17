using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDados
    {
        Notificador notificador = new Notificador();

        private const string enderecoBanco =
            "Data Source=(LocalDB)\\MSSQLLocalDB;" +
            "Initial Catalog=ControleMedicamentosDb;" +
            "Integrated Security=True;" +
            "Pooling=False";

        private const string sqlInserir =
            @"INSERT INTO [TBPACIENTE]
                (
                    [NOME],       
                    [CARTAOSUS]
                )
            VALUES
                (
                    @NOME,
                    @CARTAOSUS
                ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @" UPDATE [TBPACIENTE]
                    SET 
                        [NOME] = @NOME, 
                        [CARTAOSUS] = @CARTAOSUS

                    WHERE [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBREQUISICAO]
                WHERE [PACIENTE_ID] = @ID

                DELETE FROM [TBPACIENTE] 
                    WHERE [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                [ID],       
                [NOME],
                [CARTAOSUS]
            FROM
                [TBPACIENTE]";

        private const string sqlSelecionarPorNumero =
            @"SELECT 
                [ID],       
                [NOME],
                [CARTAOSUS]
            FROM
                [TBPACIENTE]
            WHERE 
                [ID] = @ID";


        public ValidationResult Inserir(Paciente novoRegistro)
        {
            var validador = new ValidadorPaciente();

            var resultadoValidacao = validador.Validate(novoRegistro);

            if (resultadoValidacao.IsValid == false)
            {
                notificador.ApresentarMensagem(resultadoValidacao.ToString(), TipoMensagem.Atencao);
                return resultadoValidacao;
            }

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosPaciente(novoRegistro, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoRegistro.id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Paciente registro)
        {
            var validador = new ValidadorPaciente();

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            var x = SelecionarPorNumero(registro.id);

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosPaciente(registro, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Paciente registro)
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

        public Paciente SelecionarPorNumero(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            Paciente paciente = null;
            if (leitorPaciente.Read())
                paciente = ConverterParaPaciente(leitorPaciente);

            conexaoComBanco.Close();

            return paciente;
        }

        public List<Paciente> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            List<Paciente> pacientes = new List<Paciente>();

            while (leitorPaciente.Read())
            {
                Paciente paciente = ConverterParaPaciente(leitorPaciente);

                pacientes.Add(paciente);
            }

            conexaoComBanco.Close();

            return pacientes;
        }

        #region Métodos privados

        private void ConfigurarParametrosPaciente(Paciente paciente, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", paciente.id);
            comando.Parameters.AddWithValue("NOME", paciente.Nome);
            comando.Parameters.AddWithValue("CARTAOSUS", paciente.CartaoSUS);
        }

        private Paciente ConverterParaPaciente(SqlDataReader leitorPaciente)
        {
            var id = Convert.ToInt32(leitorPaciente["ID"]);
            var nome = Convert.ToString(leitorPaciente["NOME"]);
            var cartaoSus = Convert.ToString(leitorPaciente["CARTAOSUS"]);

            Paciente paciente = new Paciente();
            paciente.id = id;
            paciente.Nome = nome;
            paciente.CartaoSUS = cartaoSus;

            return paciente;
        }

        #endregion

    }
}
