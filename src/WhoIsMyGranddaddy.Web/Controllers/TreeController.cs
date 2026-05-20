using Microsoft.AspNetCore.Mvc;
using WhoIsMyGranddaddy.Core;
using WhoIsMyGranddaddy.Web.Models;

namespace WhoIsMyGranddaddy.Web.Controllers;

public sealed class TreeController(GenealogyService genealogy) : Controller
{
    public IActionResult Index() => View();

    public async Task<IActionResult> Search(string identityNumber, int depth = 10)
    {
        var id = identityNumber?.Trim() ?? "";
        if (!SaIdNumber.IsValid(id))
            return PartialView("_TreeResult", new TreeViewModel
            {
                IdentityNumber = id,
                MaxDepth = depth,
                Notice = "Enter a valid 13-digit South African ID number.",
            });

        var tree = await genealogy.GetDescendantsAsync(id, depth);
        return PartialView("_TreeResult", new TreeViewModel
        {
            IdentityNumber = id,
            MaxDepth = depth,
            Root = tree.Root,
            HasMore = tree.HasMore,
        });
    }
}
