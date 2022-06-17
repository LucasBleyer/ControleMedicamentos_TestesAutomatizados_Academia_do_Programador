using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    public class TelaCadastroPaciente : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioPacienteEmBancoDados _repositorioPaciente;
        private readonly Notificador _notificador;

        public TelaCadastroPaciente(RepositorioPacienteEmBancoDados repositorioPaciente, Notificador notificador)
            : base("Cadastro de Paciente")
        {
            _repositorioPaciente = repositorioPaciente;
            _notificador = notificador;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Paciente");

            Paciente novoPaciente = ObterPaciente();

            var resultadoValidacao = _repositorioPaciente.Inserir(novoPaciente);

            if (resultadoValidacao.IsValid)
                _notificador.ApresentarMensagem("Paciente cadastrado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Paciente");

            bool temPacientesCadastrados = VisualizarRegistros("Pesquisando");

            if (temPacientesCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum paciente cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroPaciente = ObterNumeroRegistro();

            Paciente pacienteAtualizado = ObterPaciente();

            pacienteAtualizado.id = numeroPaciente;

            _repositorioPaciente.Editar(pacienteAtualizado);

            _notificador.ApresentarMensagem("Paciente editado com sucesso", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Paciente");

            bool temPacientesRegistrados = VisualizarRegistros("Pesquisando");

            if (temPacientesRegistrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum paciente cadastrado para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroPaciente = ObterNumeroRegistro();
            Paciente paciente = ObterObjetoRegistro(numeroPaciente);

            _repositorioPaciente.Excluir(paciente);

            _notificador.ApresentarMensagem("Paciente excluído com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Paciente");

            List<Paciente> pacientes = _repositorioPaciente.SelecionarTodos();

            if (pacientes.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum paciente disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Paciente paciente in pacientes)
                Console.WriteLine(paciente.ToString());

            Console.ReadLine();

            return true;
        }

        private Paciente ObterPaciente()
        {
            Console.Write("Digite o nome do paciente: ");
            string nome = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite o Cartão SUS do paciente: ");
            string cartao = Console.ReadLine();

            return new Paciente(nome, cartao);
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            Paciente pacienteEncontrado;

            do
            {
                Console.Write("Digite o ID do paciente: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                pacienteEncontrado = _repositorioPaciente.SelecionarPorNumero(numeroRegistro);

                if (pacienteEncontrado == null)
                    _notificador.ApresentarMensagem("ID do paciente não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (pacienteEncontrado == null);

            return numeroRegistro;
        }

        public Paciente ObterObjetoRegistro(int numeroRegistro)
        {
            Paciente pacienteEncontrado = _repositorioPaciente.SelecionarPorNumero(numeroRegistro);

            return pacienteEncontrado;
        }
    }
}
