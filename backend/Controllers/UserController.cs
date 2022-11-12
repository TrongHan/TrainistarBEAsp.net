using backend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ManagerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("all")]
        [HttpGet]
        public JsonResult getAllUser()
        {
            //Tạo câu query
            string query = @"select * from Manager";
            //Hứng data query về table
            DataTable table = new DataTable();
            //Lấy chuỗi string connect vào db (setup ở appsettings.json)
            string data = _configuration.GetConnectionString("DBConnect");
            //Tạo con reader data mysql
            MySqlDataReader reader;
            //Gọi connect tới mysql
            using(MySqlConnection con=new MySqlConnection(data))
            {
                //Mở connection
                con.Open();
                //Execute script mysql
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    reader = cmd.ExecuteReader();
                    //Load data về table
                    table.Load(reader);
                    //Dóng connection
                    reader.Close();
                    con.Close();
                }
            }
            //Paste data từ table về dưới dạng json
            return new JsonResult(table);
        }

        [Route("auth")]
        [HttpPost]
        public JsonResult Login(ManagerDTO user)
        {
            string query = @"select * from Manager 
            where username=@username and password=@password
            ";
            DataTable table = new DataTable();
            string data = _configuration.GetConnectionString("DBConnect");
            MySqlDataReader reader;
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
            
            return new JsonResult(table);
        }

    }
}
