using Ninject;
using Parliament.Api.SGNS.ViewModels;
using Parliament.DataAccess.Database;
using Parliament.DataAccess.Entities;
using Parliament.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Parliament.Api.SGNS.Endpoints
{
    [Authorize]
    public class UsersController : ApiController
    {
        [Inject]
        public IRepository<ParliamentDbContext, ParliamentUser> UserRepo { get; set; }

        [HttpGet]
        [Route("api/users/username/{username}", Name = "GetUserByUsername")]
        public async Task<IHttpActionResult> GetUserByUserName(string username)
        {
            var user = (await UserRepo.GetAsync(u => u.UserName == username)).SingleOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("user-lookup", string.Format("'{0}' does not exist", username));
                return BadRequest(ModelState);
            }

            return Ok(new UserInfoViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }
    }
}