using System;
using System.Collections.Generic;
using NhiberteConfiguration = NHibernate.Cfg.Configuration;

namespace CleanTemplate.Data.NHibernate
{
    /// <summary>
    /// Parameters for creating a nhibernate session/connection to database
    /// </summary>
    public class SessionFactoryConfiguration
    {
        public DatabaseType DatabaseType { get; set; }

        public string ConnectionString { get; set; }

        /// <summary>
        /// If enabled, NHibernate will output all SQL commands executed.
        /// </summary>
        public bool ShowSql { get; set; }

        public void AddFluentMapping(Type mapping)
        {
            Mappings.Add(mapping);
        }

        public IList<Type> Mappings { get; } = new List<Type>();

        public void Configuration(Action<NhiberteConfiguration> configuration)
        {
            ActionConfiguration = configuration;
        }

        internal Action<NhiberteConfiguration> ActionConfiguration { get; private set; }
    }
}
