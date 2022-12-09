using backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Course_StudentController : ControllerBase
    {

        BaseResponse response = new BaseResponse();
        private readonly IConfiguration _configuration;
        public Course_StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("{idUser}")]
        [HttpGet]
        public JsonResult getStudentResult(string idUser)
        {
            //, [FromBody]Course_StudentDTO course_student
            string query = @"select * 
            from User_ U, Course_Student CS
            where typeUser = 1 and U.idUser = CS.idStudent and U.idUser = @idUser";
            
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;

            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idUser", idUser);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
