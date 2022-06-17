using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    public class TelaCadastroRequisicao : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioRequisicaoEmBancoDados repositorioRequisicao;
        private readonly RepositorioPacienteEmBancoDados _repositorioPaciente = new RepositorioPacienteEmBancoDados();
        private readonly RepositorioMedicamentoEmBancoDados _repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
        private readonly RepositorioFuncionarioEmBancoDados _repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();
        private readonly TelaCadastroPaciente telaCadastroPaciente;
        private readonly TelaCadastroMedicamento telaCadastroMedicamento;
        private readonly TelaCadastroFuncionario telaCadastroFuncionario;
        private readonly Notificador _notificador;

        public TelaCadastroRequisicao(
            RepositorioRequisicaoEmBancoDados repositorioRequisicao,
            RepositorioPacienteEmBancoDados repositorioPaciente,
            RepositorioMedicamentoEmBancoDados repositorioMedicamento,
            RepositorioFuncionarioEmBancoDados repositorioFuncionario,
            TelaCadastroPaciente telaCadastroPaciente,
            TelaCadastroMedicamento telaCadastroMedicamento,
            TelaCadastroFuncionario telaCadastroFuncionario,
            Notificador notificador) : base("Cadastro de Requisicao")
        {
            this.repositorioRequisicao = repositorioRequisicao;
            this._repositorioPaciente = repositorioPaciente;
            this._repositorioMedicamento = repositorioMedicamento;
            this._repositorioFuncionario = repositorioFuncionario;
            this.telaCadastroPaciente = telaCadastroPaciente;
            this.telaCadastroMedicamento = telaCadastroMedicamento;
            this.telaCadastroFuncionario = telaCadastroFuncionario;
            this._notificador = notificador;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Requisição");

            Requisicao novaRequisicao = ObterRequisicao();

            if (novaRequisicao == null)
            {
                return;
            }

            if (novaRequisicao.Medicamento.QuantidadeDisponivel >= novaRequisicao.QtdMedicamento)
            {

                var resultadoValidacao = repositorioRequisicao.Inserir(novaRequisicao);

                if (resultadoValidacao.IsValid)
                {
                    novaRequisicao.Medicamento.QuantidadeDisponivel = novaRequisicao.Medicamento.QuantidadeDisponivel - novaRequisicao.QtdMedicamento;
                    if (novaRequisicao.Medicamento.QuantidadeDisponivel == 0)
                    {
                        _repositorioMedicamento.Excluir(novaRequisicao.Medicamento);
                        _notificador.ApresentarMensagem("Requisição cadastrada com sucesso!", TipoMensagem.Sucesso);
                    }
                    else
                    {
                        _repositorioMedicamento.Editar(novaRequisicao.Medicamento);
                        _notificador.ApresentarMensagem("Requisição cadastrada com sucesso!", TipoMensagem.Sucesso);
                    }
                }
            }
            else
            {
                _notificador.ApresentarMensagem("Não há medicamentos o suficiente", TipoMensagem.Erro);
            }
        }

        public void Editar()
        {
            MostrarTitulo("Editando Requisição");

            bool temRequisicoesCadastradas = VisualizarRegistros("Pesquisando");

            if (temRequisicoesCadastradas == false)
            {
                _notificador.ApresentarMensagem("Nenhuma requisição cadastrada para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroRequisicao = ObterNumeroRegistro();
            Requisicao requisicaoAtualizada = ObterRequisicao();

            requisicaoAtualizada.id = numeroRequisicao;

            repositorioRequisicao.Editar(requisicaoAtualizada);

            _notificador.ApresentarMensagem("Requisição editada com sucesso", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Requisição");

            bool temRequisicoesRegistradas = VisualizarRegistros("Pesquisando");

            if (temRequisicoesRegistradas == false)
            {
                _notificador.ApresentarMensagem("Nenhuma requisição cadastrada para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroRequisicao = ObterNumeroRegistro();
            Requisicao requisicao = ObterObjetoRegistro(numeroRequisicao);

            repositorioRequisicao.Excluir(requisicao);

            _notificador.ApresentarMensagem("Requisição excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Requisições");

            List<Requisicao> requisicoes = repositorioRequisicao.SelecionarTodos();

            if (requisicoes.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhuma requisição disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Requisicao requisicao in requisicoes)
                Console.WriteLine(requisicao.ToString());

            Console.ReadLine();

            return true;
        }

        private Requisicao ObterRequisicao()
        {
            Medicamento medicamentoSelecionado = ObtemMedicamento();

            Console.Write("Digite a quantidade de medicamento: ");
            int quantidadeMedicamento = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            Paciente pacienteSelecionado = ObtemPaciente();

            DateTime dataHora = DateTime.Now;

            Funcionario funcionarioSelecionado = ObtemFuncionario();

            if (medicamentoSelecionado == null || funcionarioSelecionado == null || pacienteSelecionado == null)
            {
                _notificador.ApresentarMensagem("Você deve preencher os dados corretamente", TipoMensagem.Erro);
                return null;
            }

            return new Requisicao(medicamentoSelecionado, pacienteSelecionado, quantidadeMedicamento, dataHora, funcionarioSelecionado);
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            Requisicao numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID da requisição: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = repositorioRequisicao.SelecionarPorNumero(numeroRegistro);

                if (numeroRegistroEncontrado == null)
                    _notificador.ApresentarMensagem("ID da requisição não foi encontrada, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == null);

            return numeroRegistro;
        }

        public Requisicao ObterObjetoRegistro(int numeroRegistro)
        {
            Requisicao requisicaoEncontrada = repositorioRequisicao.SelecionarPorNumero(numeroRegistro);

            return requisicaoEncontrada;
        }

        private Paciente ObtemPaciente()
        {
            Console.WriteLine("Pacientes");
            bool temPacienteDisponivel = telaCadastroPaciente.VisualizarRegistros("Pesquisando");

            if (!temPacienteDisponivel)
            {
                _notificador.ApresentarMensagem("Não há nenhum paciente cadastrado.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o ID do paciente: ");
            int idPaciente = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Paciente pacienteSelecionado = _repositorioPaciente.SelecionarPorNumero(idPaciente);

            return pacienteSelecionado;
        }

        private Medicamento ObtemMedicamento()
        {
            Console.WriteLine("Medicamentos");
            bool temMedicamentoDisponivel = telaCadastroMedicamento.VisualizarRegistros("Pesquisando");

            if (!temMedicamentoDisponivel)
            {
                _notificador.ApresentarMensagem("Não há nenhum medicamento cadastrado.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o ID do medicamento: ");
            int idMedicamento = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Medicamento medicamentoSelecionado = _repositorioMedicamento.SelecionarPorNumero(idMedicamento);

            return medicamentoSelecionado;
        }

        private Funcionario ObtemFuncionario()
        {
            Console.WriteLine("Funcionários");
            bool temFuncionarioDisponivel = telaCadastroFuncionario.VisualizarRegistros("Pesquisando");

            if (!temFuncionarioDisponivel)
            {
                _notificador.ApresentarMensagem("Não há nenhum funcionário cadastrado.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o ID do funcionário: ");
            int idFuncionario = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Funcionario funcionarioSelecionado = _repositorioFuncionario.SelecionarPorNumero(idFuncionario);

            return funcionarioSelecionado;
        }
    }
}
