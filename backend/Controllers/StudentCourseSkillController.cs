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
    public class StudentCourseSkillController : ControllerBase
    {
        BaseResponse response = new BaseResponse();
        private readonly IConfiguration _configuration;
        public StudentCourseSkillController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("skill/{idStudent}")]
        [HttpGet]
        public JsonResult getSkill(string idStudent)
        {
            string query = @"select Course_Student.idStudent, user_.firstName, user_.lastName, course.nameCourse, Skill.nameSkill
                from Course_Student 
                    join user_ on user_.idUser=Course_Student.idStudent
                    join Course_Skill on course_student.idCourse=course_skill.idCourse
                    join Skill on course_skill.idSkill=skill.idSkill
                    join Course on course.idCourse=course_student.idCourse
                where idStudent=@idStudent";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idStudent", idStudent);
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
