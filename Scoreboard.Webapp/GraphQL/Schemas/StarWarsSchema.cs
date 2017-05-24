using System;
using GraphQL.Types;
using Scoreboard.Webapp.GraphQL.Queries;

namespace Scoreboard.Webapp.GraphQL.Schemas
{
    public class StarWarsSchema : Schema
    {
        public StarWarsSchema(Func<Type, GraphType> resolveType) : base(resolveType)
        {
            Query = (StarWarsQuery)resolveType(typeof(StarWarsQuery));
        }
    }
}
