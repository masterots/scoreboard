using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace Scoreboard.Webapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SetupGraphQL();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        private static async void SetupGraphQL()
        {
            Console.WriteLine("Hello GraphQL!");

            var schema = new Schema { Query = new StarWarsQuery() };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = @"
                    query {
                        hero {
                            id
                            name
                        }
                    }
                ";
            }).ConfigureAwait(false);

            var json = new DocumentWriter(indent: true).Write(result);

            Console.WriteLine(json);
        }

        public class Droid
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class DroidType : ObjectGraphType<Droid>
        {
            public DroidType()
            {
                Field(x => x.Id).Description("The Id of the Droid");
                Field(x => x.Name, nullable: true).Description("The Name of the Droid");
            }
        }

        public class StarWarsQuery : ObjectGraphType
        {
            public StarWarsQuery()
            {
                Field<DroidType>(
                    "hero",
                    resolve: context => new Droid { Id = "1", Name = "R2-D2" }
                );
            }
        }
    }
}
