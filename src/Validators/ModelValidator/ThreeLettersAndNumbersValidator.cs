using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace ModelValidator;

[Guid("f3a3a3a3-3a3a-3a3a-3a3a-3a3a3a3a3a3a")]
public sealed class ThreeLettersAndNumbersValidator : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        var modelParsed = modelo.Value;
        return Regex.IsMatch(modelParsed, @"^[A-Za-z]{3}\d{3}$");
    }
}
