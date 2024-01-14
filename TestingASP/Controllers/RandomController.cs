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

        public RandomController(ILogger<RandomController> logger)
        {
            data = new Dictionary<int, RandomData>();
            users = new Dictionary<int, string>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(i,new RandomData("test",i,Random.Shared.Next().ToString()));
            }
            _logger = logger;
            loadUsers();

        }

        private void loadUsers(){
            using (TextFieldParser parser = new TextFieldParser(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(),"data"),"logins.csv")))
            {
                parser.SetDelimiters(",");
                while(!parser.EndOfData){
                    string[]? info = parser.ReadFields();
                    users.Add(int.Parse(info[0]),info[1]);
                   _logger.LogInformation(String.Join(",",info));
                }
            }
        }

        [HttpGet("GetRandom/{id}",Name = "GetRandom")]
        [ProducesResponseType(typeof(RandomData), 200)]
        public IActionResult GetRandom(string id)
        {
            int idNum = int.Parse(id);
            return Check(idNum,data);
        }

        [HttpGet("GetUser/{id}",Name = "GetUser")]
        public IActionResult GetUser(string id)
        {
            int idNum = int.Parse(id);
            return Check(idNum,users);
        }

        public IActionResult Check<T>(int id, Dictionary<int,T> dictionary)
        {
            _logger.LogInformation(dictionary[id]?.ToString());
            if(users.ContainsKey(id))
                return Ok(dictionary[id]?.ToString());
            return NotFound();
        }
}