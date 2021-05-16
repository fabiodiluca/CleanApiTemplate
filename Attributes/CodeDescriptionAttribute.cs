using System;

namespace CleanTemplate.Attributes
{
    public class CodeDescriptionAttribute : Attribute
    {
        public CodeDescriptionAttribute(string codigo, string descricao)
        {
            this.Codigo = codigo;
            this.Descricao = descricao;
        }

        public string Codigo { get; }
        public string Descricao { get; }
    }
}
