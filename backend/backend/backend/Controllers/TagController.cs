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
    public class TagController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public TagController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // hiển thị danh sách Tag
        [HttpGet]
        public JsonResult Get()
        {
            
            string query = @"
                SELECT ta.TagId, ta.TagName, ta.Color,
                JSON_ARRAYAGG(t.TaskTitle) AS TaskList
                FROM Tags AS ta
                LEFT JOIN task_tags AS tt ON tt.TagId = ta.TagId
                LEFT JOIN Tasks    AS t  ON t.TaskId  = tt.TaskId
                GROUP BY ta.TagId, ta.TagName, ta.Color;
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


        // POST api/<TagController>
        [HttpPost]
        public JsonResult Post(Tag Tag)
        {
            string query = @"
                Insert into Tags(TagName,Color) values (@TagName,@Color)
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TagName", Tag.TagName);
                    myCommand.Parameters.AddWithValue("@Color", Tag.Color);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("added successfully");
        }

        // PUT api/<TagController>/5
        [HttpPut("{id}")]
        public JsonResult Put(Tag Tag, int id)
        {
            string query = @"
                Update Tags set TagName = @TagName, Color = @Color where TagId = @TagId;
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TagId", id);
                    myCommand.Parameters.AddWithValue("@TagName", Tag.TagName);
                    myCommand.Parameters.AddWithValue("@Color", Tag.Color);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("updated successfully");
        }

        // DELETE api/<TagController>/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                Delete from Tags where TagId = @TagId;
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TagId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("deleted successfully");
        }
    }
}
