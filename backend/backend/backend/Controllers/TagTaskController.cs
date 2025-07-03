using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.IO;
using backend.Models;
using System.Text.Json;
using System.Runtime.Intrinsics.Arm;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagTaskController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public TagTaskController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // hiển thị danh sách TagTask
        [HttpGet]
        public JsonResult Get()
        {

            string query = @"
                Select * from task_tags
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }


        // POST api/<TagTaskController>
        [HttpPost]
        public JsonResult Post(TagTask TagTask)
        {
            string query = @"
                Insert into task_tags(TaskId,TagId) values (@TaskId,@TagId)
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TaskId", TagTask.TaskId);
                    myCommand.Parameters.AddWithValue("@TagId", TagTask.TagId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("added successfully");
        }
    }
}
