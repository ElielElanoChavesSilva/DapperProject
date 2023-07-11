using Dapper;
using DapperExtensions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string? _connectionString;

        public ComentarioController(IConfiguration configuration)
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
        public ActionResult<IEnumerable<Comentario>> GetAll()
        {
            var sql = "SELECT * FROM Comentario";
            using (var connect = Connection())
            {
                return Ok(connect.Query<Comentario>(sql));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Comentario> GetById([FromRoute] int id)
        {
            var sql = "SELECT * FROM Comentario c WhERE c.Id = @Id";
            using (var connect = Connection())
            {
                var comentario = connect.QuerySingleOrDefault<Comentario>(sql, new { id });
                if(comentario == null)
                {
                    NotFound();
                }
                return comentario;
            }
        }

        [HttpPost]
        public ActionResult<Comentario> Post([FromBody] Comentario obj)
        {
            var sql = "Insert into Comentario(Texto, DataC, IdPerfil, IdPost) values (@Texto, @DataC, @IdPerfil, @IdPost)";
            using (var connect = Connection())
            {
                connect.Execute(sql, new
                {
                    obj.Texto,
                    obj.DataC,
                    obj.IdPerfil,
                    obj.IdPost
                });
            }
            return StatusCode(201);
        }

        [HttpPut]
        public ActionResult Put([FromBody] Comentario obj)
        {
            using (var connect = Connection())
            {
                return Ok(connect.Update(obj));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var sql = "DELETE Comentario FROM Comentario WHERE Id = @Id; ";
            using (var connect = Connection())
            {
                return Ok(connect.Execute(sql, new { id }));
            }
        }
    }
}