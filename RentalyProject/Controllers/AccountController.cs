﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentalyProject.Interfaces;
using RentalyProject.Models;
using RentalyProject.Utilities.Enums;
using RentalyProject.Utilities.Exceptions;
using RentalyProject.Utilities.Extensions;
using RentalyProject.ViewModels.Account;

namespace RentalyProject.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, RoleManager<IdentityRole> roleManager,IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM newUser, string? returnUrl)
        {
            if (!ModelState.IsValid) return View();
            newUser.Name = newUser.Name.Capitalize();
            newUser.Surname = newUser.Surname.Capitalize();
            AppUser user = _mapper.Map<AppUser>(newUser);
            IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user,UserRole.Member.ToString());
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, Email = user.Email }, Request.Scheme);
            await _emailService.SendMail(user.Email,"Email Confirmation",confirmationLink);
            //await _signInManager.SignInAsync(user, false);
            //if (returnUrl != null) return Redirect(returnUrl);

            return RedirectToAction(nameof(SuccessfulRegister), "Account");
        }
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new NotFoundException("User is not found!");
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new TokenException("Token is invalid");
            }
            await _signInManager.SignInAsync(user,false);
            return View();
        }
        public IActionResult SuccessfulRegister()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM user, string? returnUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser existed = await _userManager.FindByEmailAsync(user.UsernameOrEmail);
            if (existed == null)
            {
                existed = await _userManager.FindByNameAsync(user.UsernameOrEmail);
                if (existed == null)
                {
                    ModelState.AddModelError(String.Empty, "Username or password is invalid");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(existed, user.Password, false, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Login is not enable now,please try again later");
                return View();
            }
            if (!existed.EmailConfirmed)
            {
                ModelState.AddModelError(String.Empty, "Please confirm your email");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username or password is invalid");
                return View();
            }
            if (returnUrl != null) return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout(string? returnUrl)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!(await _roleManager.RoleExistsAsync(role.ToString())))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });

                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
