using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Mvc;
using Scoreboard.Webapp.GraphQL.Schemas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scoreboard.Webapp.Controllers
{
    [Route("api/[controller]")]
    public class GraphQLController : Controller
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _writer;
        private readonly IDictionary<string, string> _namedQueries;

        public GraphQLController(
            IDocumentExecuter executer,
            IDocumentWriter writer,
            StarWarsSchema schema)
        {
            _executer = executer;
            _writer = writer;
            _schema = schema;

            _namedQueries = new Dictionary<string, string>
            {
                ["a-query"] = @"query foo { hero { name } }"
            };
        }

        [HttpGet]
        public Task<ContentResult> Get(HttpRequestMessage request)
        {
            return Post(request, new GraphQLQuery { Query = "query foo { hero }", Variables = "" });
        }

        [HttpPost]
        public async Task<ContentResult> Post(HttpRequestMessage request, GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();
            var queryToExecute = query.Query;

            if (!string.IsNullOrWhiteSpace(query.NamedQuery))
            {
                queryToExecute = _namedQueries[query.NamedQuery];
            }

            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = queryToExecute;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;

                _.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };
                _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();
            }).ConfigureAwait(false);

            var httpResult = result.Errors?.Count > 0
                                   ? 400
                                   : 200;

            var json = _writer.Write(result);

            var response = new ContentResult
            {
                StatusCode = httpResult,
                Content = new StringContent(json, System.Text.Encoding.UTF8).ToString(),
                ContentType = "application/json"
            };

            return response;
        }

        public class GraphQLQuery
        {
            public string OperationName { get; set; }
            public string NamedQuery { get; set; }
            public string Query { get; set; }
            public string Variables { get; set; }
        }
    }
}
