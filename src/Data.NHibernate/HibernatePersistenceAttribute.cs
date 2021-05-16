using System;

namespace CleanTemplate.Data.NHibernate
{
    /// <summary>
    /// Atributo usado para identificar classes que representam persistência de dados.
    /// Usado para efetuar o mapeamento (classe - banco de dados) através do fluent nhibernate
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class HibernatePersistenceAttribute : Attribute
    {
    }
}
