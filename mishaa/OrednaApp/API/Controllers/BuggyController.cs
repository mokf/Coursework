using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly DataBaseContext _dataBaseContext;

    public BuggyController(DataBaseContext dataBaseContext)
    {
        _dataBaseContext = dataBaseContext;
    }

    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = _dataBaseContext.Users.Find(-1);

        if (thing == null)
            return NotFound();

        return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        // try
        // {
        var thing = _dataBaseContext.Users.Find(-1);

        var thingToReturn = thing.ToString();

        return thingToReturn;
        // }
        // catch (Exception exception)
        // {
        //     return StatusCode(500, "Server error");
        // }
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was a bad request");
    }
}