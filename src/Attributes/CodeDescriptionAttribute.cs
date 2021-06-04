using System;

namespace CleanTemplate.Attributes
{
    public class CodeDescriptionAttribute : Attribute
    {
        public CodeDescriptionAttribute(string code, string descricao)
        {
            this.Code = code;
            this.Description = descricao;
        }

        public string Code { get; }
        public string Description { get; }
    }
}
