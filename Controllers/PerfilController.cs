using Dapper;
using DapperExtensions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string? _connectionString;

        public PerfilController(IConfiguration configuration)
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
        public ActionResult<IEnumerable<Perfil>> GetAll()
        {
            var sql = "SELECT * FROM Perfil";

            using (var connect = Connection())
            {
                return Ok(connect.Query<Perfil>(sql));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Perfil> GetById([FromRoute] int id)
        {
            var sql = "SELECT * FROM Perfil p WhERE p.Id = @Id";
            using (var connect = Connection())
            {
                var perfil = connect.QuerySingleOrDefault<Perfil>(sql, new { id });
                if(perfil == null)
                {
                    NotFound();
                }
                return perfil;

            }
        }

        [HttpPost]
        public ActionResult<Perfil> Post([FromBody] Perfil obj)
        {
            var sql = "Insert into Perfil(Nome, DataNascimento) values (@Nome, @DataNascimento)";
                using (var connect = Connection())
                {
                    connect.Execute(sql, new
                    {
                        obj.Nome,
                        obj.DataNascimento
                    });
                }
                return StatusCode(201);
        }

        [HttpPut]
        public ActionResult Update([FromBody] Perfil obj)
        {
            using (var connect = Connection())
            {
                return Ok(connect.Update(obj));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var sql = "DELETE Perfil FROM Perfil WHERE Id = @Id; ";
            using (var connect = Connection())
            {
                return Ok(connect.Execute(sql, new { id }));
            }
        }
    }
}