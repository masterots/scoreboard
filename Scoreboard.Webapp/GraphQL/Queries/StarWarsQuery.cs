using System;
using GraphQL.Types;
using Scoreboard.Webapp.GraphQL.Data;
using Scoreboard.Webapp.GraphQL.Types;

namespace Scoreboard.Webapp.GraphQL.Queries
{
    public class StarWarsQuery : ObjectGraphType<object>
    {
        public StarWarsQuery(StarWarsData data)
        {
            Name = "Query";

            Field<CharacterInterface>(
                "hero", 
                resolve: context => data.GetDroidById("3")
            );

			Field<HumanType>(
				"human",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
				),
				resolve: context => data.GetHumanById(context.GetArgument<string>("id"))
			);

            Field<DroidType>(
				"droid",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
				),
                resolve: context => data.GetDroidById(context.GetArgument<string>("id"))
			);
		}
    }
}
