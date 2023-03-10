using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCASM2.Data;
using MVCASM2.Constants;
using MVCASM2.Models;

namespace MVCASM2.Controllers
{
    public class OwnerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHash;

        public OwnerController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHash)
        {
            _db = db;
            _userManager = userManager;
            _passwordHash = passwordHash;
        }

        // GET: OwerController
        public async Task<ActionResult> Index()
        {
            var user = await _userManager.GetUsersInRoleAsync(Roles.Owner.ToString());
            return View(user);
        }

        // GET: OwerController/Edit/5
        public ActionResult<UserResetPassword> Edit(string id)
        {
            //var user = await _userManager.GetUser(id);
            var user = _db.Users.SingleOrDefault(u => u.Id == id);
            UserResetPassword userResetPassword = new UserResetPassword()
            {
                Id = user.Id,
                Email = user.Email,
            };
            return View(userResetPassword);
        }

        // POST: OwerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserResetPassword userResetPassword)
        {
            ApplicationUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {

                if (!string.IsNullOrEmpty(userResetPassword.NewPassword))
                {
                    appUser.PasswordHash = _passwordHash.HashPassword(appUser, userResetPassword.NewPassword);
                }
                else
                {
                    ModelState.AddModelError("", "Password cannot be empty");
                }

                if (!string.IsNullOrEmpty(userResetPassword.NewPassword))
                {
                    IdentityResult result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Errors(result);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        // GET: OwerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OwerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
