using System;
using System.Reflection;

namespace CleanTemplate.Attributes
{
    public static class EnumExtensions
    {
        public static CodeDescriptionAttribute CodigoDescricao(this Enum en)
        {
            var retorno = en.ToString();
            var attribute = en
                .GetType()
                .GetField(retorno)
                .GetCustomAttribute(typeof(CodeDescriptionAttribute)) as CodeDescriptionAttribute;

            return attribute;
        }
    }
}
