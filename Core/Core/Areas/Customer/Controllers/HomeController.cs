using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Data.Repository.IRepository;
using Core.Extentions;
using Core.Models;
using Core.Models.ViewModels;
using Core.Utility;

namespace Core.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _iUnitOfWork;
        private HomeViewModel HomeVM;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork iUnitOfWork)
        {
            _logger = logger;
            _iUnitOfWork = iUnitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM = new HomeViewModel()
            {
                CategoryList = _iUnitOfWork.Category.GetAll(),
                ServiceList = _iUnitOfWork.Service.GetAll(inculdeProperties: "Frequency")
            };

            return View(HomeVM);
        }

        public IActionResult Details(int Id)
        {
            var ServiceFromDb = _iUnitOfWork.Service.GetFirstOrDefualt(inculdeProperties: "Category,Frequency", filter: c => c.Id == Id);
            return View(ServiceFromDb);
        }

        public IActionResult AddToCart(int serviceId)
        {
            List<int> sessionList = new List<int>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                sessionList.Add(serviceId);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

                if (!sessionList.Contains(serviceId))
                {
                    sessionList.Add(serviceId);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
