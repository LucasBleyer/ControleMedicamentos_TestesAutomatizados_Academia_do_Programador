using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.ConsoleApp.ModuloFuncionario
{
    public class TelaCadastroFuncionario : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioFuncionarioEmBancoDados _repositorioFuncionario;
        private readonly Notificador _notificador;

        public TelaCadastroFuncionario(RepositorioFuncionarioEmBancoDados repositorioFuncionario, Notificador notificador)
            : base("Cadastro de Funcionários")
        {
            _repositorioFuncionario = repositorioFuncionario;
            _notificador = notificador;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Funcionário");

            Funcionario novoFuncionrio = ObterFuncionario();

            var resultadoValidacao = _repositorioFuncionario.Inserir(novoFuncionrio);

            if (resultadoValidacao.IsValid)
                _notificador.ApresentarMensagem("Funcionário cadastrado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Funcionario");

            bool temFuncionariosCadastrados = VisualizarRegistros("Pesquisando");

            if (temFuncionariosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum funcionário cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroFuncionario = ObterNumeroRegistro();

            Funcionario funcionarioAtualizado = ObterFuncionario();

            funcionarioAtualizado.id = numeroFuncionario;

            _repositorioFuncionario.Editar(funcionarioAtualizado);

            _notificador.ApresentarMensagem("Funcionario editado com sucesso", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Funcionário");

            bool temFuncionariosRegistrados = VisualizarRegistros("Pesquisando");

            if (temFuncionariosRegistrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum funcionário cadastrado para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroFuncionario = ObterNumeroRegistro();
            Funcionario funcionario = ObterObjetoRegistro(numeroFuncionario);

            _repositorioFuncionario.Excluir(funcionario);

            _notificador.ApresentarMensagem("Funcionario excluído com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Funcionários");

            List<Funcionario> funcionarios = _repositorioFuncionario.SelecionarTodos();

            if (funcionarios.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum funcionário disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Funcionario funcionario in funcionarios)
                Console.WriteLine(funcionario.ToString());

            Console.ReadLine();

            return true;
        }

        private Funcionario ObterFuncionario()
        {
            Console.Write("Digite o nome do funcionário: ");
            string nome = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite o login do funcionário: ");
            string login = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite a senha do funcionário: ");
            string senha = Console.ReadLine();

            return new Funcionario(nome, login, senha);
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            Funcionario funcionarioEncontrado;

            do
            {
                Console.Write("Digite o ID do Funcionário: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                funcionarioEncontrado = _repositorioFuncionario.SelecionarPorNumero(numeroRegistro);

                if (funcionarioEncontrado == null)
                    _notificador.ApresentarMensagem("ID do Funcionário não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (funcionarioEncontrado == null);

            return numeroRegistro;
        }

        public Funcionario ObterObjetoRegistro(int numeroRegistro)
        {
            Funcionario funcionarioEncontrado = _repositorioFuncionario.SelecionarPorNumero(numeroRegistro);

            return funcionarioEncontrado;
        }
    }
}
