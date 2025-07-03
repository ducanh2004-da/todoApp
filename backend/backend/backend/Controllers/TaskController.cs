using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using backend.Models;
using System.Text.Json;
using System.Runtime.Intrinsics.Arm;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public TaskController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // hiển thị danh sách task có filter(all/done/pending)
        [HttpGet]
        public JsonResult Get([FromQuery] string status = "all")
        {
            string queryFilter = status.ToLower() switch
            {
                "pending" => "Where IsDone = 0",
                "done" => "Where IsDone = 1",
                _ => ""
            };
            string query = $@"
                SELECT t.TaskId, t.TaskTitle, t.Description, t.Priority, t.StartDay, t.EndDay,
                COALESCE(JSON_ARRAYAGG(ta.TagName), JSON_ARRAY()) AS TagList
                FROM Tasks AS t
                LEFT JOIN task_tags AS tt ON tt.TaskId = t.TaskId
                LEFT JOIN Tags AS ta ON ta.TagId  = tt.TagId
                {queryFilter}                    
                GROUP BY t.TaskId, t.TaskTitle, t.Description, t.Priority, t.StartDay, t.EndDay
                ORDER BY t.StartDay DESC, t.Priority DESC;
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

        // GET api/<TaskController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            string query = @"
                Select * from Tasks Where TaskId = @TaskId;
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TaskId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            //Nếu không tìm thấy thì trả về chuỗi rỗng 
            if (table.Rows.Count == 0)
                return "";

            //Lấy row đầu và map vào object 
            var row = table.Rows[0];
            var obj = new
            {
                TaskId = Convert.ToUInt32(row["TaskId"]),
                TaskTitle = row["TaskTitle"].ToString(),
                Description = row["Description"] == DBNull.Value
                              ? null
                              : row["Description"].ToString(),
                Priority = row["Priority"].ToString(),
                StartDay = row["StartDay"] == DBNull.Value
                              ? null
                              : ((DateTime)row["StartDay"]).ToString("yyyy-MM-dd"),
                EndDay = row["EndDay"] == DBNull.Value
                              ? null
                              : ((DateTime)row["EndDay"]).ToString("yyyy-MM-dd"),
                IsDone = Convert.ToBoolean(row["IsDone"]),
                Created_at = (DateTime)row["Created_at"],
                Updated_at = (DateTime)row["Updated_at"]
            };
            return JsonSerializer.Serialize(obj);
        }

        // POST api/<TaskController>
        [HttpPost]
        public JsonResult Post(Task task)
        {
            string query = @"
                Insert into Tasks(TaskTitle,Description,Priority,StartDay,EndDay) values (@TaskTitle,@Description,@Priority,@StartDay,@EndDay)
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TaskTitle", task.TaskTitle);
                    myCommand.Parameters.AddWithValue("@Description", task.Description);
                    myCommand.Parameters.AddWithValue("@Priority", task.Priority);
                    myCommand.Parameters.AddWithValue("@StartDay", task.StartDay);
                    myCommand.Parameters.AddWithValue("@EndDay", task.EndDay);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("added successfully");
        }

        // PUT api/<TaskController>/5
        [HttpPut("{id}")]
        public JsonResult Put(Task task, int id)
        {
            string query = @"
                Update Tasks set TaskTitle = @TaskTitle, Description = @Description, Priority = @Priority, StartDay = @StartDay, EndDay = @EndDay where TaskId = @TaskId;
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TaskId", id);
                    myCommand.Parameters.AddWithValue("@TaskTitle", task.TaskTitle);
                    myCommand.Parameters.AddWithValue("@Description", task.Description);
                    myCommand.Parameters.AddWithValue("@Priority", task.Priority);
                    myCommand.Parameters.AddWithValue("@StartDay", task.StartDay);
                    myCommand.Parameters.AddWithValue("@EndDay", task.EndDay);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("updated successfully");
        }

        // DELETE api/<TaskController>/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                Delete from Tasks where TaskId = @TaskId;
             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TaskId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("deleted successfully");
        }

        // để complete task
        [HttpPut("complete/{id}")]
        public JsonResult MarkComplete(int id)
        {
            string query = @"
            UPDATE Tasks SET IsDone = 1, Updated_at = CURRENT_TIMESTAMP
            WHERE TaskId = @TaskId;";

            string sqlDataSource = _configuration.GetConnectionString("TaskAppCon");
            int rowsAffected;

            using (var conn = new MySqlConnection(sqlDataSource))
            using (var myCommand = new MySqlCommand(query, conn))
            {
                myCommand.Parameters.AddWithValue("@TaskId", id);

                conn.Open();
                rowsAffected = myCommand.ExecuteNonQuery();
            }

            if (rowsAffected == 0)
                return new JsonResult("Không cập nhật thành công");

            return new JsonResult("Cập nhật thành công");
        }

        // thống kê báo cáo task
        [HttpGet("report")]
        public JsonResult GetReport()
        {
            string query = @"
                SELECT COUNT(*) AS total_tasks,SUM(IsDone = 1) AS DoneTasks,
                SUM(IsDone = 0) AS PendingTasks,
                SUM(Priority='High')  AS HighPriority,
                SUM(Priority='Medium') AS MediumPriority,
                SUM(Priority='Low') AS LowPriority FROM tasks;";

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
    }
}
