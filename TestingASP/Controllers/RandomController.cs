using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;

namespace TestingASP.Controllers;
[ApiController]
[Route("api/Random")]
public class RandomController : ControllerBase
{
        private readonly ILogger<RandomController> _logger;
        private Dictionary<int, RandomData> data;
        private Dictionary<int, string> users;
        private string datapath;

        public RandomController(ILogger<RandomController> logger)
        {
            data = new Dictionary<int, RandomData>();
            users = new Dictionary<int, string>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(i,new RandomData("test",i,Random.Shared.Next().ToString()));
            }
            datapath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(),"data"),"logins.csv");
            _logger = logger;
            loadUsers();

        }

        private void loadUsers(){
            using (TextFieldParser parser = new TextFieldParser(datapath))
            {
                parser.SetDelimiters(",");
                while(!parser.EndOfData){
                    string[]? info = parser.ReadFields();
                    try{
                        users.Add(int.Parse(info[0]),info[1]);
                    _logger.LogInformation(string.Join(",",info));
                    }catch(Exception exception){
                        _logger.LogError(exception.ToString());
                    }
                }
            }
        }

        [HttpGet("Get/Random/{id}",Name = "GetRandom")]
        [ProducesResponseType(typeof(RandomData), 200)]
        public ActionResult GetRandom(string id)
        {
            int idNum = int.Parse(id);
            return Check(idNum,data);
        }

        [HttpGet("Get/User/{id}",Name = "GetUser")]
        public ActionResult GetUser(string id)
        {
            int idNum = int.Parse(id);
            return Check(idNum,users);
        }
        
        [HttpGet("Add/User",Name = "AddUser")]
        public async Task<ActionResult> AddUser([FromQuery] string user,[FromQuery] string pass)
        {
            int id = users.Keys.Max()+1;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                _logger.LogWarning("Invalid user or pass when adding user");
                return BadRequest("Invalid user or pass");
            }

            await using(StreamWriter writer = new StreamWriter(datapath)){
                try{
                    _logger.LogInformation("Attempt to create user: " + id + "," + user + "," + pass);
                    writer.WriteLine(id + "," + user + "," + pass);
                    _logger.LogInformation("User Created ID: " + id);
                    return Ok("Id: "+id.ToString());
                }catch (Exception exception) {
                    _logger.LogError(exception.ToString());
                    return  BadRequest(exception.ToString()); 
                }
            }
        }

        public ActionResult Check<T>(int id, Dictionary<int,T> dictionary)
        {
            _logger.LogInformation(dictionary[id]?.ToString());
            if(users.ContainsKey(id))
                return Ok(dictionary[id]?.ToString());
            return NotFound();
        }
}