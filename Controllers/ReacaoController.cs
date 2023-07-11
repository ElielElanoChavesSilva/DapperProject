using Dapper;
using DapperExtensions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using static Slapper.AutoMapper;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReacaoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string? _connectionString;

        public ReacaoController(IConfiguration configuration)
        {

            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        protected static SqlConnection Connection()
        {
            var connection = new SqlConnection(_connectionString);
            return connection;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Reacao>> GetAll()
        {
            var sql = "SELECT * FROM Reacao";
            using (var connect = Connection())
            {
                return Ok(connect.Query<Reacao>(sql));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Reacao> GetById([FromRoute] int id)
        {
            var sql = "SELECT * FROM Reacao r WhERE r.Id = @Id";
            using (var connect = Connection())
            {
                var reacao = connect.QuerySingleOrDefault<Reacao>(sql, new { id });
                if(reacao == null)
                {
                    NotFound();
                }
                return reacao;
            }
        }

        [HttpPost]
        public ActionResult<Reacao> Post([FromBody] Reacao obj)
        {
            var sql = "Insert into Reacao(Reacoes, IdPerfil, IdPost) values (@Reacoes, @IdPerfil, @IdPost);";
            using (var connect = Connection())
            {
                connect.Execute(sql, new
                {
                    obj.Reacoes,
                    obj.IdPerfil,
                    obj.IdPost
                });
            }
            return StatusCode(201);
        }

        [HttpPut]
        public ActionResult Put([FromBody] Reacao obj)
        {
            using (var connect = Connection())
            {
                return Ok(connect.Update(obj));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var sql = "DELETE Reacao FROM Reacao WHERE Id = @Id; ";
            using (var connect = Connection())
            {
                return Ok(connect.Execute(sql, new { id }));
            }
        }
    }
}