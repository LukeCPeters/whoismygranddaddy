using Microsoft.AspNetCore.Mvc;

namespace WhoIsMyGranddaddy.Web.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index() => View();
}
