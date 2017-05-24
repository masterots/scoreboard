using System;
using GraphQL.Types;

namespace Scoreboard.Webapp.GraphQL.Types
{
    public class CharacterInterface : InterfaceGraphType<StarWarsCharacter>
    {
        public CharacterInterface()
        {
        }
    }
}
