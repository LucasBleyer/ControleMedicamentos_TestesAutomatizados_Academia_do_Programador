using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class ValidadorMedicamento : AbstractValidator<Medicamento>
    {
        public ValidadorMedicamento()
        {
            RuleFor(x => x.Nome)
            .NotNull().NotEmpty().MinimumLength(3);

            RuleFor(x => x.Descricao)
            .NotNull().NotEmpty().MinimumLength(3);

            RuleFor(x => x.Lote)
            .NotNull().NotEmpty().MinimumLength(1);

            RuleFor(x => x.Fornecedor)
            .NotNull();
        }
    }
}
