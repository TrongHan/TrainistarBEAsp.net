using backend.Model;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Xml.Linq;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        BaseResponse response = new BaseResponse();
        private readonly IConfiguration _configuration;
        public CourseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public String getNextCourseId()
        {
            string query = @"select max(convert(idCourse,signed)) from Course";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            string courseId = "";
            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    courseId = table.Rows[0][0].ToString();
                    reader.Close();
                    con.Close();
                }
            }
            int temp=Int32.Parse(courseId)+1;
            courseId=temp.ToString();
            return courseId;
        }

        [Route("all")]
        [HttpGet]
        public JsonResult getAllCourseInfo()
        {
            string query = @"select course.*,manager.fullname as managerName, Concat(user_.firstName,' ',user_.lastname) as teacherName from ((course left join manager on course.idManager=manager.id)left join user_ on course.idTeacher=user_.idUser )";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }

        [Route("searchname/{name}")]
        [HttpGet]
        public JsonResult getAllCourseInfoByName(string name)
        {
            string query = @"select * from Course where nameCourse like @name";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            name="%"+name+"%";
            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }
        [Route("searchid/{id}")]
        [HttpGet]
        public JsonResult getAllCourseInfoById(string id)
        {
            string query = @"select * from Course where idCourse like @id";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }
        [Route("createcourse")]
        [HttpPost]
        public JsonResult createCourse(CourseDTO course) {
            string query = @"insert into Course values (
            @idCourse,            
            @idTeacher,
            @idManager,
            @nameCourse,
            @description,
            @idQuestionBank,
            @startDate,
            @finishDate
            )";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            try
            {
                using (MySqlConnection con = new MySqlConnection(data))
                {
                    con.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        
                        course.idCourse = getNextCourseId();
                        cmd.Parameters.AddWithValue("@idCourse", course.idCourse);
                        cmd.Parameters.AddWithValue("@idTeacher", course.idTeacher);
                        cmd.Parameters.AddWithValue("@idManager", course.idManager);
                        cmd.Parameters.AddWithValue("@nameCourse", course.nameCourse);
                        cmd.Parameters.AddWithValue("@description", course.description);
                        cmd.Parameters.AddWithValue("@idQuestionBank", course.idQuestionBank);
                        cmd.Parameters.AddWithValue("@startDate", course.startDate);
                        cmd.Parameters.AddWithValue("@finishDate", course.finishDate);
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
                response.message = "Create user failed";
            }
            response.code = "1";
            response.message = "Create succeeded";
            return new JsonResult(response);
        }
        [Route("updatecourse/{id}")]
        [HttpPatch]
        public JsonResult updateCourse(string id, [FromBody]CourseDTO course)
        {
            string query = @"update Course set            
            idTeacher=@idTeacher,
            idManager=@idManager,
            nameCourse=@nameCourse,
            description=@description,
            idQuestionBank=@idQuestionBank,
            startDate=@startDate,
            finishDate=@finishDate
            where idCourse=@idCourse";
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

                        course.idCourse =id;
                        cmd.Parameters.AddWithValue("@idCourse", course.idCourse);
                        cmd.Parameters.AddWithValue("@idTeacher", course.idTeacher);
                        cmd.Parameters.AddWithValue("@idManager", course.idManager);
                        cmd.Parameters.AddWithValue("@nameCourse", course.nameCourse);
                        cmd.Parameters.AddWithValue("@description", course.description);
                        cmd.Parameters.AddWithValue("@idQuestionBank", course.idQuestionBank);
                        cmd.Parameters.AddWithValue("@startDate", course.startDate);
                        cmd.Parameters.AddWithValue("@finishDate", course.finishDate);
                        reader = cmd.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                        con.Close();
                    }
                }
            return new JsonResult(response);
        }
        [Route("deletecourse/{id}")]
        [HttpDelete]
        public JsonResult deleteCourseInfoById(string id)
        {
            string query = @"delete * from Course where idCourse like @id";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            response.code = "1";
            response.message = "Delete succeeded";
            try
            {
                using (MySqlConnection con = new MySqlConnection(data))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
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
                response.message = "Delete user failed";
            }
            return new JsonResult(response);
        }
    }
    
}
