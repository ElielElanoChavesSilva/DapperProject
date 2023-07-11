using Dapper;
using DapperExtensions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


namespace DapperProject.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        private static string? _connectionString;

        public LinkController(IConfiguration configuration)
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
        public  ActionResult <IEnumerable<Link>> GetAll()
        {
            var sql = "SELECT * FROM Link";
            using (var connect = Connection())
            {
                return Ok(connect.Query<Link>(sql));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Link> GetById([FromRoute] int id)
        {
            var sql = "SELECT * FROM Link l WhERE l.Id = @Id";
            using (var connect = Connection())
            {
               var link = connect.QuerySingleOrDefault<Link>(sql, new { id });
                if(link == null)
                {
                    NotFound();
                }
                return link;
            }
        }

        [HttpPost]
        public ActionResult<Link> Post([FromBody] Link obj)
        {
            var sql = "insert into Link(Texto, IdPost) values (@Texto, @IdPost);";
            using (var connect = Connection())
            {
                connect.Execute(sql, new 
                { 
                    obj.Texto,
                    obj.IdPost
                });
            }
            return StatusCode(201);
        }

        [HttpPut]
        public ActionResult Put([FromBody] Link obj)
        {
            using (var connect = Connection())
            {
                return Ok(connect.Update(obj));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var sql = "Delete l from Link l where Id = @Id";
            using (var connect = Connection())
            {
                return Ok(connect.Execute(sql, new { id }));
            }
        }
    }
}
