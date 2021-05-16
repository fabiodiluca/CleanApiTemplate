using System;
using System.Collections.Generic;

namespace CleanTemplate.Attributes
{
    public class DatabaseSourceAttribute: Attribute
    {
        public readonly List<DatabaseSource> Sources;

        public DatabaseSourceAttribute(params DatabaseSource[] sources)
        {
            Sources = new List<DatabaseSource>(sources);
        }
    }
}
