using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.DataAccess.Data.Repository.IRepository;

namespace Core.DataAccess.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void LockUser(string UserId)
        {
            var UserFromDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == UserId);
            UserFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        public void UnLockUser(string UserId)
        {
            var UserFromDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == UserId);
            UserFromDb.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
