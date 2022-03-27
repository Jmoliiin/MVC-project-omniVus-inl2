using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Omnivus_2.Models;
using Omnivus_2.Models.Data;
using Omnivus_2.Models.Entites;
using Omnivus_2.Models.Forms;
using Omnivus_2.Services;
using System.Security.Claims;

namespace Omnivus_2.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileManager _profileManager;
        private readonly AppDbContext _context;

        public ProfileController(IProfileManager profileManager, AppDbContext context)
        {
            _profileManager = profileManager;
            _context = context;
        }



        [HttpGet("{id}")]//Ska ta emot ett id
        [Route("profile/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var userID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            
            if (userID == id)//Kontrollera att inloggad id är samma som begärd url id
            {
                var profile = await _profileManager.ReadAsync(id);
                return View(profile);
            }
            return RedirectToAction("AccessDenied", "Authentication");



        }

        [Route("profile/edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var userID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userID == id)//Kontrollera att inloggad id är samma som begärd url id
            {
                var userProfileEntity = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
                var userProfile = new UserProfile
                {
                    FirstName = userProfileEntity.FirstName,
                    LastName = userProfileEntity.LastName,
                    StreetName = userProfileEntity.StreetName,
                    PostalCode = userProfileEntity.PostalCode,
                    Country = userProfileEntity.Country,
                    City = userProfileEntity.City,


                };

                return View(userProfile);
            }
            return RedirectToAction("AccessDenied", "Authentication");
            

        }
        [Route("profile/edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UpdateUserProfile profile)
        {
            try
            {
                var userProfileEntity = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == id);


                userProfileEntity.UserId = id;
                userProfileEntity.FirstName = profile.FirstName;
                userProfileEntity.LastName = profile.LastName;
                userProfileEntity.StreetName = profile.StreetName;
                userProfileEntity.PostalCode = profile.PostalCode;
                userProfileEntity.Country = profile.Country;
                userProfileEntity.City = profile.City;


                _context.Profiles.Update(userProfileEntity);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Profile", new { id = id });
            }
            catch
            {
                return View(profile);
            }

        }
    }
}
