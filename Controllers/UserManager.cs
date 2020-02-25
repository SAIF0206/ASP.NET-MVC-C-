using AD_Project.DbContext;
using AD_Project.Models.Departments;
using System.Linq;

/*
Keerthana
 */

namespace AD_Project.Controllers
{
    internal class UserManager
    {
        public ApplicationDbContext applicationDbContext { get; set; }
        public UserManager()
        {
            applicationDbContext = new ApplicationDbContext();
        }

        public User IsValid(string username, string password)
        {
            if (username.StartsWith("emp") || username.StartsWith("rep") || username.StartsWith("head"))
            {
                var user = applicationDbContext.Dept_Staffs.SingleOrDefault(m => m.Username == username);
                return user;
            }
            else
            {
                var user = applicationDbContext.Store_Staffs.SingleOrDefault(m => m.Username == username);
                return user;
            }
        }

        public static string EncodePasswordToBase64(string password)
        {
            return password;
        }
    }
}
