using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    public class TelaCadastroMedicamento : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioMedicamentoEmBancoDados _repositorioMedicamento;
        private readonly RepositorioFornecedorEmBancoDados _repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
        private readonly Notificador _notificador;

        public TelaCadastroMedicamento(RepositorioMedicamentoEmBancoDados repositorioMedicamento, Notificador notificador)
            : base("Cadastro de Medicamento")
        {
            _repositorioMedicamento = repositorioMedicamento;
            _notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Visualizar Medicamentos em Falta");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Medicamento");

            Medicamento novoMedicamento = ObterMedicamento();

            if (novoMedicamento == null)
            {
                return;
            }

            var resultadoValidacao = _repositorioMedicamento.Inserir(novoMedicamento);

            if (resultadoValidacao.IsValid)
                _notificador.ApresentarMensagem("Medicamento cadastrado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Medicamento");

            bool temMedicamentosCadastrados = VisualizarRegistros("Pesquisando");

            if (temMedicamentosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum medicamento cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroMedicamento = ObterNumeroRegistro();
            Medicamento medicamentoAtualizado = ObterMedicamento();

            medicamentoAtualizado.id = numeroMedicamento;

            _repositorioMedicamento.Editar(medicamentoAtualizado);

            _notificador.ApresentarMensagem("Medicamento editado com sucesso", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Medicamento");

            bool temMedicamentosRegistrados = VisualizarRegistros("Pesquisando");

            if (temMedicamentosRegistrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum medicamento cadastrado para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroMedicamento = ObterNumeroRegistro();
            Medicamento medicamento = ObterObjetoRegistro(numeroMedicamento);

            _repositorioMedicamento.Excluir(medicamento);

            _notificador.ApresentarMensagem("Medicamento excluído com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Medicamento");

            List<Medicamento> medicamentos = _repositorioMedicamento.SelecionarTodos();

            if (medicamentos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum medicamento disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Medicamento medicamento in medicamentos)
                Console.WriteLine(medicamento.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarFornecedores(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Fornecedores");

            List<Fornecedor> fornecedores = _repositorioFornecedor.SelecionarTodos();

            if (fornecedores.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum fornecedor disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.WriteLine("Fornecedores");

            foreach (Fornecedor fornecedor in fornecedores)
                Console.WriteLine(fornecedor.ToString());

            Console.ReadLine();

            return true;
        }

        private Medicamento ObterMedicamento()
        {
            Console.Write("Digite o nome do medicamento: ");
            string nome = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite a descrição do medicamento: ");
            string descricao = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite o lote do medicamento: ");
            string lote = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Digite a validade do medicamento: ");
            DateTime validade = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Digite a quantidade disponivel: ");
            int quantidade = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            VisualizarFornecedores("Pesquisando");

            Console.Write("Digite o ID do fornecedor: ");
            int idFornecedor = Convert.ToInt32(Console.ReadLine());

            Fornecedor fornecedorSelecionado = _repositorioFornecedor.SelecionarPorNumero(idFornecedor);

            if (fornecedorSelecionado == null)
            {
                _notificador.ApresentarMensagem("Você deve preencher os dados corretamente", TipoMensagem.Erro);
                return null;
            }

            return new Medicamento(nome, descricao, lote, validade, quantidade, fornecedorSelecionado);
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            Medicamento medicamentoEncontrado;

            do
            {
                Console.Write("Digite o ID do medicamento: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                medicamentoEncontrado = _repositorioMedicamento.SelecionarPorNumero(numeroRegistro);

                if (medicamentoEncontrado == null)
                    _notificador.ApresentarMensagem("ID do medicamento não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (medicamentoEncontrado == null);

            return numeroRegistro;
        }

        public Medicamento ObterObjetoRegistro(int numeroRegistro)
        {
            Medicamento medicamentoEncontrado = _repositorioMedicamento.SelecionarPorNumero(numeroRegistro);

            return medicamentoEncontrado;
        }

        public void MostrarMedicamentosEmFalta()
        {
            MostrarTitulo("Medicamentos em Falta");

            List<Medicamento> medicamentos = _repositorioMedicamento.SelecionarTodos();

            foreach (Medicamento medicamento in medicamentos)

                if (medicamento.QuantidadeDisponivel <= 10)
                {
                    Console.WriteLine(medicamento.ToString());
                }

            Console.ReadLine();

        }
    }
}
