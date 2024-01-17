using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;

namespace TestingASP.Controllers;
[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
        private readonly ILogger<LoginController> _logger;
        private Dictionary<int, RandomData> data;
        private Dictionary<int, User> users;
        private string datapath;

        public LoginController(ILogger<LoginController> logger)
        {
            data = new Dictionary<int, RandomData>();
            users = new Dictionary<int, User>();
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
                        users.Add(int.Parse(info[0]), new User(int.Parse(info[0]),info[1],info[2]));
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

        [HttpGet("Get/User",Name = "GetUser")]
        public ActionResult GetUser([FromQuery] string? id, [FromQuery] string? user = "",[FromQuery] string? pass = "")
        {
            _logger.LogInformation("tst");
            if(!string.IsNullOrEmpty(id)){  
                int idNum = int.Parse(id);
                return Check(idNum,users);
            }
            else if(!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass)){
                pass = HashString(pass);
                foreach (User value in users.Values)
                {
                    if (value.match(user,pass)){
                        return Ok(value);
                    }
                }
                return NotFound();
            }
            return BadRequest();
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

            await using(StreamWriter writer = new StreamWriter(datapath,true)){
                try
                {
                    string password = HashString(pass);
                    _logger.LogInformation("Attempt to create user: " + id + "," + user + "," + password);
                    writer.WriteLine(id + "," + user + "," + password);
                    users.Add(id, new User(id,user,password));
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
            if(dictionary.ContainsKey(id)){
                _logger.LogInformation(dictionary[id]?.ToString());
                return Ok(dictionary[id]);
            }
            return NotFound();
        }

        private string HashString (string bytes)
        {
            HashAlgorithm sha256 = SHA256.Create();
            byte[] result = sha256.ComputeHash(Encoding.ASCII.GetBytes(bytes));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte value in result)
            {
                stringBuilder.Append(value.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
}