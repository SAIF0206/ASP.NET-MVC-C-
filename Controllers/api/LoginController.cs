using AD_Project.DbContext;
using System.Web.Http;

/*
    MOHD SAIF ANSARI
*/

namespace AD_Project.Controllers.api
{
    public class LoginController : ApiController
    {
        private ApplicationDbContext _context;

        public LoginController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public IHttpActionResult CheckUsers(string Username, string Password)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var User = new UserManager().IsValid(Username, Password);
            return Ok(User);


        }



        /* // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }*/
    }
}
