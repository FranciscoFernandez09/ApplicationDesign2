using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace ModelValidator;

[Guid("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")]
public sealed class SixLetterValidator : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        var modelParsed = modelo.Value;
        return Regex.IsMatch(modelParsed, @"^[A-Za-z]{6}$");
    }
}
