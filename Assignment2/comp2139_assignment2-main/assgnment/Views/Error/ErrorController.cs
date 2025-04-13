public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult Error404
    {
        return View("Error404");
    }

    [Route("Error/500")]
    public IActionResult Error500()
    {
        return View("Error500");
    }

    [Route("Error/[{code}")]
    public IActionResult ErrorGeneric(int code)
    {
        return View("GenericError", code);
    }
}