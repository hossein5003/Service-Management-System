using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
namespace Core.DataAccess.Data.Repository.IRepository
{
    public interface IUserRepository :IRepository<ApplicationUser>
    {
        void LockUser(string UserId);

        void UnLockUser(string UserId);
    }
}
