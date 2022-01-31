using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Core.Utility;

namespace Core.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.Admin)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _iUnitOfWork;

        public UserController(IUnitOfWork iUnitOfWork)
        {
            _iUnitOfWork = iUnitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return View(_iUnitOfWork.User.GetAll(u=>u.Id!=claims.Value));
        }

        public IActionResult Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _iUnitOfWork.User.LockUser(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _iUnitOfWork.User.UnLockUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

