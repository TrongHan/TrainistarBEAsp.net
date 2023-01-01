﻿using backend.Model;
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
        [Route("history/{idstudent}")]
        [HttpGet]
        public JsonResult getHistory(string idstudent)
        {
            string query = @"select course.idCourse, course.nameCourse, course_student.mark from course_student
            join course on course.idCourse=course_student.idCourse
            where course_student.idStudent=@idstudent and course_student.mark IS NOT null";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idstudent", idstudent);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }
        [Route("mark/{idStudent}/{idCourse}")]
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
                    cmd.Parameters.AddWithValue("@mark", course_student.mark);
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

        [Route("totalstudents/month")]
        [HttpGet]
        public JsonResult getTotalStudentsInCourseByMonth()
        {
            //, [FromBody]Course_StudentDTO course_student
            string query = @"
            SELECT CS.idCourse, C.nameCourse, count(CS.idStudent) AS TotalStudents, MONTH(C.startDate) AS Month
            FROM trainistar.course_student CS, trainistar.course C
            WHERE CS.idCourse = C.idCourse
            GROUP BY CS.idCourse;";

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

        [Route("totalstudents/quarter")]
        [HttpGet]
        public JsonResult getTotalStudentsInCourseByQuarter()
        {
            //, [FromBody]Course_StudentDTO course_student
            string query = @"
            SELECT CS.idCourse, C.nameCourse, count(CS.idStudent) AS TotalStudents, QUARTER(C.startDate) AS Quarters
            FROM trainistar.course_student CS, trainistar.course C
            WHERE CS.idCourse = C.idCourse
            GROUP BY CS.idCourse";

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

        [Route("totalstudents/year")]
        [HttpGet]
        public JsonResult getTotalStudentsInCourseByYear()
        {
            //, [FromBody]Course_StudentDTO course_student
            string query = @"
            SELECT CS.idCourse, C.nameCourse, count(CS.idStudent) AS TotalStudents, YEAR(C.startDate) AS Years
            FROM trainistar.course_student CS, trainistar.course C
            WHERE CS.idCourse = C.idCourse
            GROUP BY CS.idCourse";

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
    }
}

