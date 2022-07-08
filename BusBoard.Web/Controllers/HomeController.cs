using Microsoft.AspNetCore.Mvc;
using BusBoard.Web.Models;
using BusBoard.Web.ViewModels;

namespace BusBoard.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TransportApi _transportApi = new TransportApi();
    private readonly PostcodeApi _postcodeApi = new PostcodeApi();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(PostcodeSelection selection)
    {
        if (!ModelState.IsValid)
        {
            return View(selection);
        }

        return RedirectToAction(nameof(BusInfo), selection);
    }

    [HttpGet]
    public async Task<ActionResult> BusInfo(PostcodeSelection selection)
    {
        var postcodeResponse = await _postcodeApi.GetLatitudeAndLongitude(selection.Postcode);
        var nearbyStops = await _transportApi.Get2NearestStops(postcodeResponse);
        var stopsAndDepartures = await _transportApi.GetDeparturesForStops(nearbyStops);

        var info = new BusInfo(postcodeResponse.Postcode, stopsAndDepartures);
        return View(info);
    }

    public ActionResult About()
    {
        ViewBag.Message = "Information about this site";

        return View();
    }

    public ActionResult Contact()
    {
        ViewBag.Message = "Contact us!";

        return View();
    }
}
