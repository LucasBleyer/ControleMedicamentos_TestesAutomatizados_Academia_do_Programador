using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class ValidadorFuncionario : AbstractValidator<Funcionario>
    {
        public ValidadorFuncionario()
        {
            RuleFor(x => x.Nome)
            .NotNull().NotEmpty().MinimumLength(3);

            RuleFor(x => x.Login)
            .NotNull().NotEmpty().MinimumLength(3);

            RuleFor(x => x.Senha)
            .NotNull().NotEmpty().MinimumLength(3);

        }
    }
}
