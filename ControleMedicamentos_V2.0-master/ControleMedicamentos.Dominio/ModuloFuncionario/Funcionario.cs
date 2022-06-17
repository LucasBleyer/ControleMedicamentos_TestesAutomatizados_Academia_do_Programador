namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class Funcionario : EntidadeBase
    {
        public Funcionario()
        {
        }

        public Funcionario(string nome, string login, string senha)
        {
            Nome = nome;
            Login = login;
            Senha = senha;
        }

        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Funcionario funcionario &&
                   id == funcionario.id &&
                   Nome == funcionario.Nome &&
                   Login == funcionario.Login &&
                   Senha == funcionario.Senha;
        }

        public override string ToString()
        {
            return $"{id}{" - "}{Nome}";
        }


    }
}
