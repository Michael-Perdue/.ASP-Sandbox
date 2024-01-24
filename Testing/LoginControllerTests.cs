using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using TestingASP.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using TestingASP;
using Xunit;

namespace Testing;



public class LoginControllerTests
{

    private LoginController GetNewController(QueryCollection queryCollection)
    {
        Mock<ILogger<LoginController>> loggerMock = new Mock<ILogger<LoginController>>();
        return new LoginController(loggerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Query = queryCollection}
                }
            }
        };
    }

    [Fact]
    public void GetRandom_Returns_OkResult()
    {
        var testData = new Dictionary<int, RandomData>();
        testData.Add(1,new RandomData("Testing", 1, "RandomValue"));
        LoginController controller = GetNewController(new QueryCollection());
        controller.data = testData; 
        ActionResult result = controller.GetRandom("1");
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, (result as OkObjectResult)?.StatusCode);
    }
    
    [Fact]
    public void GetRandom_Returns_NotFoundResult()
    {
        LoginController controller = GetNewController(new QueryCollection());
        ActionResult result = controller.GetRandom("100");
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, (result as NotFoundResult)?.StatusCode);
    }
    
    [Fact]
    public void GetUser_Returns_NotFoundResult()
    {
        QueryCollection query = new QueryCollection(new Dictionary<string, StringValues>
        {
            ["id"] = "100",
            ["user"] = "noaccount",
            ["pass"] = "noaccount"
        });
        LoginController controller = GetNewController(query);
        
        ActionResult result = controller.GetUser("100");
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, (result as NotFoundResult)?.StatusCode);
        result = controller.GetUser(id: null,user: "noaccount", pass: "noaccount");
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, (result as NotFoundResult)?.StatusCode);
    }
    
    [Fact]
    public void GetUserApiCall_Returns_NotFoundResult()
    {
        LoginController controller = GetNewController(new QueryCollection());
        ActionResult result = controller.GetUser("100");
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, (result as NotFoundResult)?.StatusCode);
        result = controller.GetUser(null,"noaccount","noaccount");
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, (result as NotFoundResult)?.StatusCode);
    }
}