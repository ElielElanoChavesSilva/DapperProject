using Dapper;
using DapperExtensions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


namespace DapperProject.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ImagemController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private static string? _connectionString;

        public ImagemController(IConfiguration configuration)
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
        public ActionResult<IEnumerable<Imagem>> GetAll()
        {
            var sql = "SELECT * FROM Imagem";
            using (var connect = Connection())
            {
                return Ok(connect.Query<Imagem>(sql));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Imagem> GetById([FromRoute] int id)
        {
            var sql = "SELECT * FROM Imagem i WhERE i.Id = @Id";
            using (var connect = Connection())
            {
                   var imagem = connect.QuerySingleOrDefault<Imagem>(sql, new { id });
                if (imagem == null)
                {
                    NotFound();
                }
                return imagem;
            }
        }

        [HttpPost]
        public ActionResult<Imagem> Post([FromBody] Imagem obj)
        {
            var sql = "insert into Imagem(IdPost, Caminho) values (@IdPost,@Caminho)";

            using (var connect = Connection())
            {
                connect.Execute(sql, new
                {
                    obj.IdPost,
                    obj.Caminho
                });
            }
            return StatusCode(201);
        }

        [HttpPut]
        public ActionResult Put([FromBody] Imagem obj)
        {
            using (var connect = Connection())
            {
                return Ok(connect.Update(obj));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var sql = "Delete i from Imagem i where Id = @Id";
            using (var connect = Connection())
            {
                return Ok(connect.Execute(sql, new { id }));
            }
        }
    }
}
