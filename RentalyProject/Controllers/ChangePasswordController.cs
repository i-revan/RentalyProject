using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentalyProject.Interfaces;
using RentalyProject.Models;
using RentalyProject.ViewModels;

namespace RentalyProject.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public ChangePasswordController(UserManager<AppUser> userManager,IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            AppUser user = await _userManager.FindByEmailAsync(forgotPasswordVM.Mail);
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetTokenLink = Url.Action("ResetPassword","ChangePassword",new
            {
                userId = user.Id,
                token = passwordResetToken
            },HttpContext.Request.Scheme);
            await _emailService.SendMail(user.Email, "Reset Password", passwordResetTokenLink);

            return View();
        }
        public IActionResult ResetPassword(string userId,string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];
            if(userId is null || token is null)
            {

            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ResetPasswordAsync(user,token.ToString(),resetPasswordVM.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();

        }
    }
}
