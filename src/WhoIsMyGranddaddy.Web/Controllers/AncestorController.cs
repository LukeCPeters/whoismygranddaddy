using Microsoft.AspNetCore.Mvc;
using WhoIsMyGranddaddy.Core;
using WhoIsMyGranddaddy.Web.Models;

namespace WhoIsMyGranddaddy.Web.Controllers;

public sealed class AncestorController(GenealogyService genealogy) : Controller
{
    public IActionResult Index() => View();

    public async Task<IActionResult> Search(string identityNumber)
    {
        var id = identityNumber?.Trim() ?? "";
        if (!SaIdNumber.IsValid(id))
            return PartialView("_Result", new AncestorViewModel
            {
                Notice = "Enter a valid 13-digit South African ID number.",
            });

        var result = await genealogy.FindRootAscendantAsync(id);
        return PartialView("_Result", new AncestorViewModel { Start = result.Start, Root = result.Root });
    }
}
