using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Data.Repository.IRepository;
using Core.Extentions;
using Core.Models;
using Core.Models.ViewModels;
using Core.Utility;

namespace Core.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _iUnitOfWork;

        [BindProperty]
        public CartViewModel CartVM { get; set; }

        public CartController(IUnitOfWork iUnitOfWork)
        {
            _iUnitOfWork = iUnitOfWork;
            CartVM = new CartViewModel()
            {
                OrderHeader = new Models.OrderHeader(),
                ServiceList = new List<Service>()
            };
        }

        public IActionResult Index()
        {
            var Carts = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

            if (Carts != null)
            {
                List<int> SessionList = Carts;
                    
                foreach(int ServiceId in SessionList)
                {
                    CartVM.ServiceList.Add(_iUnitOfWork.Service
                        .GetFirstOrDefualt(u => u.Id == ServiceId, inculdeProperties: "Category,Frequency"));
                }
            }

            return View(CartVM);
        }

        public IActionResult Summary()
        {
            List<int> Carts = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

            if (Carts != null)
            {
                foreach (int ServiceId in Carts)
                {
                    CartVM.ServiceList.Add(_iUnitOfWork.Service
                        .GetFirstOrDefualt(u => u.Id == ServiceId, inculdeProperties: "Category,Frequency"));
                }
            }

            return View(CartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var Carts = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            CartVM.ServiceList = new List<Service>();

            if (Carts != null)
            {
                foreach (int ServiceId in Carts)
                {
                    CartVM.ServiceList.Add(_iUnitOfWork.Service.GetFirstOrDefualt(u => u.Id == ServiceId, inculdeProperties: "Category,Frequency"));
                }
            }

            if (!ModelState.IsValid)
            {
                return View(CartVM);
            }
            else
            {
                CartVM.OrderHeader.OrderDate = DateTime.Now;
                CartVM.OrderHeader.Status = SD.StatusSubmitted;
                CartVM.OrderHeader.ServiceCount = CartVM.ServiceList.Count;

                _iUnitOfWork.OrderHeader.Add(CartVM.OrderHeader);
                _iUnitOfWork.Save();

                foreach(var item in CartVM.ServiceList)
                {
                    OrderDetails orderDetails = new OrderDetails()
                    {
                        ServiceId=item.Id,
                        OrderHeaderId=CartVM.OrderHeader.Id,
                        ServiceName=item.Name,
                        Price=item.Price
                    };

                    _iUnitOfWork.OrderDetails.Add(orderDetails);
                }

                _iUnitOfWork.Save();
                HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
                return RedirectToAction("OrderConfirmation", "Cart", new { id = CartVM.OrderHeader.Id });
            }
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        public IActionResult Remove(int serviceId)
        {
            var Carts = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            Carts.Remove(serviceId);

            HttpContext.Session.SetObject(SD.SessionCart, Carts);

            return RedirectToAction(nameof(Index));
        }
    }
}
