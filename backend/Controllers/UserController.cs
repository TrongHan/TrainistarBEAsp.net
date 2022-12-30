using backend.Function;
using backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Reflection.Emit;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        BaseResponse response = new BaseResponse();
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("{username}")]
        [HttpDelete]
        public JsonResult DeleteUser(string username)
        {
            string query = @"delete from User_ where userName=@userName";
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
                        cmd.Parameters.AddWithValue("@userName", username);
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
        [Route("{username}")]
        [HttpGet]
        public JsonResult getUser(string username)
        {
            string query = @"select * from User_ where userName=@userName";
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
        [Route("all")]
        [HttpGet]
        public JsonResult getAllUser()
        {
            string query = @"select * from User_";
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
        [Route("auth")]
        [HttpPost]
        public JsonResult Login(UserAuthentication user)
        {
            string query = @"select * from User_ 
            where userName=@username and password=@password
            ";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
            BaseResponse response = new BaseResponse();
            using (MySqlConnection con = new MySqlConnection(data))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@password", user.password);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            try
            {
                if (table.Rows[0][0].ToString() != null)
                {
                    response.code = "1";
                    response.message = "Login succeeded";

                }
            }
            catch (Exception ex)
            {
                response.code = "-1";
                response.message = "User login failed, please check your account";
            }
            return new JsonResult(response);
        }
        [Route("create")]
        [HttpPost]
        public JsonResult Login(UserDTO user)
        {
            string query = @"insert into user_ values ( 
            @idUser,
            @userName,
            @password,
            @firstName,
            @lastName,
            @email,
            @phoneNumber,
            @gender,
            @typeUser,
            @rating
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
                        Random r=new Random();
                        int randomId = r.Next(0, 100000000);
                        user.idUser=randomId.ToString();
                        cmd.Parameters.AddWithValue("@idUser", user.idUser);
                        cmd.Parameters.AddWithValue("@userName", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@firstName", user.firstName);
                        cmd.Parameters.AddWithValue("@lastName", user.lastName);
                        cmd.Parameters.AddWithValue("@email", user.email);
                        cmd.Parameters.AddWithValue("@phoneNumber", user.phoneNumber);
                        cmd.Parameters.AddWithValue("@gender", user.gender);
                        cmd.Parameters.AddWithValue("@typeUser", user.typeUser);
                        cmd.Parameters.AddWithValue("@rating", user.rating);
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
        [Route("{username}")]
        [HttpPut]
        public JsonResult UpdateUser(string username,[FromBody]UserDTO user)
        {
            string query = @"update user_ set
            password=@password,
            firstName=@firstName,
            lastName=@lastName,
            email=@email,
            phoneNumber=@phoneNumber,
            gender=@gender,
            typeUser=@typeUser,
            rating=@rating
            where userName=@username";
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
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@firstName", user.firstName);
                        cmd.Parameters.AddWithValue("@lastName", user.lastName);
                        cmd.Parameters.AddWithValue("@email", user.email);
                        cmd.Parameters.AddWithValue("@phoneNumber", user.phoneNumber);
                        cmd.Parameters.AddWithValue("@gender", user.gender);
                        cmd.Parameters.AddWithValue("@typeUser", user.typeUser);
                        cmd.Parameters.AddWithValue("@rating", user.rating);
                        cmd.Parameters.AddWithValue("@username", username);
                        reader = cmd.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                        con.Close();
                    }
                }
            return new JsonResult(response);
        }

        [Route("allstudents")]
        [HttpGet]
        public JsonResult getAllStudent()
        {
            string query = @"select * from User_ where typeUser = 1";
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
