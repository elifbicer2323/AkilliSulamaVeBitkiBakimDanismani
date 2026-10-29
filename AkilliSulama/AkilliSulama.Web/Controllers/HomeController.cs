using System.Diagnostics;
using System.Security.Claims;
using AkilliSulama.Business;
using AkilliSulama.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AkilliSulama.Web.Controllers;

public class HomeController : Controller
{
    private readonly PlanServisi _planServisi;
    private readonly ParselServisi _parselServisi;

    public HomeController(PlanServisi planServisi, ParselServisi parselServisi)
    {
        _planServisi = planServisi;
        _parselServisi = parselServisi;
    }

    // Giriş yapmışsa: özet + bugünkü işler (tüm modüller burada birleşiyor)
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            int kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var parseller = await _parselServisi.ListeleAsync(kullaniciId);

            ViewBag.ParselSayisi = parseller.Count;
            ViewBag.BitkiSayisi = parseller.Sum(p => p.Bitkiler.Count);
            ViewBag.Bugun = await _planServisi.KullaniciTakvimiAsync(kullaniciId, 1);
        }
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}