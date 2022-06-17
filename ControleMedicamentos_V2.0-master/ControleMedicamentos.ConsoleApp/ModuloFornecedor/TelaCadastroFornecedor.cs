using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.ConsoleApp.ModuloFornecedor
{
    public class TelaCadastroFornecedor : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioFornecedorEmBancoDados _repositorioFornecedor;
        private readonly Notificador _notificador;

        public TelaCadastroFornecedor(RepositorioFornecedorEmBancoDados repositorioFornecedor, Notificador notificador)
            : base("Cadastro de Fornecedores")
        {
            _repositorioFornecedor = repositorioFornecedor;
            _notificador = notificador;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Fornecedor");

            Fornecedor novoFornecedor = ObterFornecedor();

            var resultadoValidacao = _repositorioFornecedor.Inserir(novoFornecedor);

            if (resultadoValidacao.IsValid)
                _notificador.ApresentarMensagem("Fornecedor cadastrado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Fornecedor");

            bool temFornecedoresCadastrados = VisualizarRegistros("Pesquisando");

            if (temFornecedoresCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum fornecedor cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroFornecedor = ObterNumeroRegistro();

            Fornecedor fornecedorAtualizado = ObterFornecedor();

            fornecedorAtualizado.id = numeroFornecedor;

            _repositorioFornecedor.Editar(fornecedorAtualizado);

            _notificador.ApresentarMensagem("Fornecedor editado com sucesso", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Fornecedor");

            bool temFornecedoresRegistrados = VisualizarRegistros("Pesquisando");

            if (temFornecedoresRegistrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum fornecedor cadastrado para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroFornecedor = ObterNumeroRegistro();
            Fornecedor fornecedor = ObterObjetoRegistro(numeroFornecedor);

            _repositorioFornecedor.Excluir(fornecedor);

            _notificador.ApresentarMensagem("Fornecedor excluído com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Fornecedores");

            List<Fornecedor> fornecedores = _repositorioFornecedor.SelecionarTodos();

            if (fornecedores.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum fornecedor disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Fornecedor fornecedor in fornecedores)
                Console.WriteLine(fornecedor.ToString());

            Console.ReadLine();

            return true;
        }

        private Fornecedor ObterFornecedor()
        {
            Console.Write("Digite o nome do fornecedor: ");
            string nome = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite o telefone do fornecedor: ");
            string telefone = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite o email do fornecedor: ");
            string email = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite a cidade do fornecedor: ");
            string cidade = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite o estado do fornecedor: ");
            string estado = Console.ReadLine();

            return new Fornecedor(nome, telefone, email, cidade, estado);
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            Fornecedor fornecedorEncontrado;

            do
            {
                Console.Write("Digite o ID do fornecedor: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                fornecedorEncontrado = _repositorioFornecedor.SelecionarPorNumero(numeroRegistro);

                if (fornecedorEncontrado == null)
                    _notificador.ApresentarMensagem("ID do fornecedor não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (fornecedorEncontrado == null);

            return numeroRegistro;
        }

        public Fornecedor ObterObjetoRegistro(int numeroRegistro)
        {
            Fornecedor fornecedorEncontrado = _repositorioFornecedor.SelecionarPorNumero(numeroRegistro);

            return fornecedorEncontrado;
        }
    }
}
