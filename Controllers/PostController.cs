using Dapper;
using DapperExtensions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private static string? _connectionString;

        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        protected static SqlConnection Connection()
        {
            var connection = new SqlConnection(_connectionString);
            return connection;
        }

        [HttpGet("{id}")]
        public ActionResult<Post> GetById([FromRoute] int id)
        {
            var sql = "SELECT * FROM Post p Where p.id = @Id";
            using (var connect = Connection())
            {
                var post = connect.QuerySingleOrDefault<Post>(sql, new { id });
                if(post == null)
                {
                    NotFound();
                }
                return post;
            } 
        }
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll() 
        {
            var sql = "SELECT * FROM Post";
            using(var connect = Connection())
            {
                return Ok(connect.Query<Post>(sql));
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Post obj)
        {
            var sql = "Insert into Post(Titulo, Descricao, DataP, IdPerfil) values(@Titulo, @Descricao, @DataP, @IdPerfil)";
            using (var connect = Connection())
            {
                    connect.Execute(sql,new
                {
                    obj.Titulo,
                    obj.Descricao,
                    obj.DataP,
                    obj.IdPerfil
                });
            }
            return StatusCode(201);
        }

        [HttpPut]
        public ActionResult Put([FromBody] Post obj)
        {
            using (var connect = Connection())
            {
                return Ok(connect.Update(obj));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var sql = "DELETE FROM Post WHERE Id = @Id;";
            using (var connect = Connection())
            {
                return Ok(connect.Execute(sql, new { id }));
            }
        }
    }
}