using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using TestingASP.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using TestingASP;
using Xunit;

namespace Testing;



public class LoginControllerTests
{
    [Fact]
    public void GetRandom_Returns_OkResult()
    {
        Mock<ILogger<LoginController>> loggerMock = new Mock<ILogger<LoginController>>();
        var testData = new Dictionary<int, RandomData>();
        testData.Add(1,new RandomData("Testing", 1, "RandomValue"));
        LoginController controller = new LoginController(loggerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.data = testData; 
        ActionResult result = controller.GetRandom("1");
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, (result as OkObjectResult)?.StatusCode);
    }
    
    public void GetRandom_Returns_OkResult()
    {
        Mock<ILogger<LoginController>> loggerMock = new Mock<ILogger<LoginController>>();
        var testData = new Dictionary<int, RandomData>();
        testData.Add(1,new RandomData("Testing", 1, "RandomValue"));
        LoginController controller = new LoginController(loggerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.data = testData; 
        ActionResult result = controller.GetRandom("1");
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, (result as OkObjectResult)?.StatusCode);
    }
    
}