using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.Dominio.Compartilhado;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using System;

namespace ControleMedicamentos.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Notificador notificador = new Notificador();
            TelaMenuPrincipal menuPrincipal = new TelaMenuPrincipal(notificador);

            while (true)
            {
                TelaBase telaSelecionada = menuPrincipal.ObterTela();

                if (telaSelecionada is null)
                    return;

                string opcaoSelecionada = telaSelecionada.MostrarOpcoes();

                if (telaSelecionada is ITelaCadastravel)
                    GerenciarCadastroBasico(telaSelecionada, opcaoSelecionada);

            }

            static void GerenciarCadastroBasico(TelaBase telaSelecionada, string opcaoSelecionada)
            {
                ITelaCadastravel telaCadastroBasico = telaSelecionada as ITelaCadastravel;

                if (telaCadastroBasico is null)
                    return;

                if (opcaoSelecionada == "1")
                    telaCadastroBasico.Inserir();

                else if (opcaoSelecionada == "2")
                    telaCadastroBasico.Editar();

                else if (opcaoSelecionada == "3")
                    telaCadastroBasico.Excluir();

                else if (opcaoSelecionada == "4")
                    telaCadastroBasico.VisualizarRegistros("Tela");

                TelaCadastroMedicamento telaCadastroMedicamento = telaCadastroBasico as TelaCadastroMedicamento;

                if (telaCadastroMedicamento is null)
                    return;

                if (opcaoSelecionada == "5")
                    telaCadastroMedicamento.MostrarMedicamentosEmFalta();
            }


        }
    }
}
