using Microsoft.AspNetCore.Mvc;

namespace TestingASP.Controllers;
[ApiController]
[Route("api/Random")]
public class RandomController : ControllerBase
{
        private readonly ILogger<RandomController> _logger;
        private Dictionary<int, RandomData> data;

        public RandomController(ILogger<RandomController> logger)
        {
            data = new Dictionary<int, RandomData>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(i,new RandomData("test",i,Random.Shared.Next().ToString()));
            }
            _logger = logger;
        }

        [HttpGet("{id}",Name = "GetAnotherData")]
        [ProducesResponseType(typeof(RandomData), 200)]
        public IActionResult Get(string id)
        {
            return Ok(data[int.Parse(id)]);
        }
}