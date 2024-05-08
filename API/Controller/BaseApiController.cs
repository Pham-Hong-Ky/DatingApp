

using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{   
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        
    }
}