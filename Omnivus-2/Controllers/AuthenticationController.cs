using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Omnivus_2.Models;
using Omnivus_2.Models.Forms;
using Omnivus_2.Services;

namespace Omnivus_2.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //hantera roller
        private readonly RoleManager<IdentityRole> _roleManager;
        //hantera profil
        private readonly IProfileManager _profileManager;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IProfileManager profileManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _profileManager = profileManager;
        }

        #region Signup

        [Route("signup")]
        [HttpGet]
        public IActionResult SignUp(string returnUrl =null)
        {
            if (_signInManager.IsSignedIn(User)) //Om användaren är inloggad->redirect to home
                return RedirectToAction("Index", "Home");

            var form=new SignUpForm();

            if(returnUrl !=null)
                form.ReturnUrl = returnUrl;

            return View(form);
        }



        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpForm form)
        {
            if (ModelState.IsValid) //Check if form is valid
            {
                if (!await _roleManager.Roles.AnyAsync()) //if no roles exist
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("user"));
                }

                if (!await _userManager.Users.AnyAsync()) //if no users been created,make first user admin
                    form.RoleName = "admin";


                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == form.Email);//check if email allready exist
                if (user == null)
                {
                    user = new IdentityUser() //create new identity with uniq email
                    {
                        UserName = form.Email,
                        Email = form.Email
                    };

                    var userResult = await _userManager.CreateAsync(user, form.Password); //create user with password
                    if (userResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, form.RoleName); //Add role to user 

                        var profile = new UserProfile //create new profile
                        {
                            FirstName = form.FirstName,
                            LastName = form.LastName,
                            Email = user.Email,
                            StreetName = form.StreetName,
                            PostalCode = form.PostalCode,
                            City = form.City,
                            Country = form.Country
                        };

                        var profileResult = await _profileManager.CreateAsync(user, profile); //create  profile
                        if (profileResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            if (form.ReturnUrl == null || form.ReturnUrl == "/")
                                return RedirectToAction("Index", "Home");
                            else
                                return LocalRedirect(form.ReturnUrl);
                        }
                        else
                        {
                            form.ErrorMessage = "There was a problem when creating your profile. Please sign in and complete your profile registration";
                        }
                    }
                }
                else
                {
                    form.ErrorMessage = "A user with the same e-mail address already exists";
                }

            }

            return View();
        }



        #endregion

        #region SignIn

        [Route("signin")]
        [HttpGet]
        public IActionResult SignIn(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var form = new SignInForm();
            if (returnUrl != null)
                form.ReturnUrl = returnUrl;

            return View(form);
        }


        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInForm form)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, isPersistent: false, false);
                if (result.Succeeded)
                {
                    if (form.ReturnUrl == null || form.ReturnUrl == "/")
                        return RedirectToAction("Index", "Home");
                    else
                        return LocalRedirect(form.ReturnUrl);
                }
            }

            form.ErrorMessage = "Incorrect email or password";
            return View(form);
        }



        #endregion 

        [Route("signout")]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Route("access-denied")]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
