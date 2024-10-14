using Demo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class FunctionController(IFunctionClient functionClient) : ControllerBase
{
    private readonly IFunctionClient _functionClient = functionClient;

    [HttpGet(Name = "GetToFunction")]
    public async Task<IActionResult> GetToFunction()
    {
        var response = await _functionClient.GetToFunctionClientAsync();
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        return Ok("Get request to function successful.");
    }

    [HttpPost(Name = "PostToFunction")]
    public async Task<IActionResult> PostToFunction()
    {
        var response = await _functionClient.PostToFunctionClientAsync();
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        return Ok("Post request to function successful.");
    }
}
