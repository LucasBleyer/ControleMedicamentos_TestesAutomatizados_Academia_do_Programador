using ControleMedicamentos.Dominio.ModuloMedicamento;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class ValidadorFornecedor : AbstractValidator<Fornecedor>
    {
        public ValidadorFornecedor()
        {
            RuleFor(x => x.Nome)
            .NotNull().NotEmpty().MinimumLength(3);

            RuleFor(x => x.Telefone)
            .NotNull().NotEmpty().MinimumLength(9).MaximumLength(11);

            RuleFor(x => x.Email)
            .NotNull().NotEmpty().MinimumLength(3);

            RuleFor(x => x.Cidade)
            .NotNull().NotEmpty().MinimumLength(3);

            RuleFor(x => x.Estado)
            .NotNull().NotEmpty().MinimumLength(2);

        }
    }
}
