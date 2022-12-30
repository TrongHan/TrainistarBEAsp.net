using backend.Model;
using backend.Function;
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
        [Route("create")]
        [HttpPost]
        public JsonResult CreateCourse_Student(Course_StudentDTO course_student)
        {
            //Tạo câu query
            string query = @"insert into course_student values ( 
            @idCourse,
            @idStudent,
            null
            )";
            //Hứng data query về table
            DataTable table = new DataTable();
            //Lấy chuỗi string connect vào db (setup ở appsettings.json)
            string data = _configuration.GetConnectionString("DBConnect");
            //Tạo con reader data mysql
            MySqlDataReader reader;
            //Gọi connect tới mysql
            try
            {
                using (MySqlConnection con = new MySqlConnection(data))
                {
                    con.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@idCourse", course_student.idCourse);
                        cmd.Parameters.AddWithValue("@idStudent", course_student.idStudent);
                        reader = cmd.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                response.code = "-1";
                response.message = "Create this course failed";
            }
            response.code = "1";
            response.message = "Create succeeded";
            return new JsonResult(response);
        }
        [Route("history/{username}")]
        [HttpGet]
        public JsonResult getHistory(string username)
        {
            string query = @"select * from Course_Student where userName=@userName and mark IS NOT null";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userName", username);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }
        [Route("mark/{username}")]
        [HttpPut]
        public JsonResult UpdateMark(string idStudent, string idCourse, [FromBody] Course_StudentDTO course_student)
        {
            string query = @"update course_student set
            mark=@mark
            where idStudent=@idStudent and idCourse=@idCourse";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            response.code = "1";
            response.message = "Update succeeded";

            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@password", course_student.mark);
                    cmd.Parameters.AddWithValue("@idStudent", idStudent);
                    cmd.Parameters.AddWithValue("@idCourse", idCourse);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(response);
        }

        [Route("student/{idUser}")]
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

