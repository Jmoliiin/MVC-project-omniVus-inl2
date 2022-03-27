using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Omnivus_2.Models;
using Omnivus_2.Models.Data;
using Omnivus_2.Models.Entites;

namespace Omnivus_2.Services
{
    public interface IProfileManager
    {
        Task<ProfileResult> CreateAsync(IdentityUser user, UserProfile profile);
        Task<UserProfile> ReadAsync(string userId);
        Task<string> DisplayNameAsync(string userId);
        //Task<UserProfile> Update(string userId, UserProfile profile);
        Task<string> DisplayRoleAsync(string userId);
        //Task<string> Edit(string userId, UpdateUserProfile profile);
    }
    public class ProfileManager : IProfileManager
    {
        private readonly AppDbContext _context;

        public ProfileManager(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ProfileResult> CreateAsync(IdentityUser user, UserProfile profile)
        {
            
            if (await _context.Users.AnyAsync(x => x.Id == user.Id))
            {
                var _profileEntity = new ProfileEntity
                {
                    UserId = user.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    StreetName = profile.StreetName,
                    PostalCode = profile.PostalCode,
                    City = profile.City,
                    Country = profile.Country

                };
                _context.Add(_profileEntity);
                await _context.SaveChangesAsync();

                return new ProfileResult { Succeeded=true};
            }

            return new ProfileResult { Succeeded=false};
        }

        public async Task<string> DisplayNameAsync(string userId)
        {
            var result = await ReadAsync(userId);
            return $"Name: {result.FirstName} {result.LastName}";
        }
        public async Task<string> DisplayRoleAsync(string userId)
        {
            var result = await ReadAsync(userId);
            return $"[{result.UserRole}]";
        }

        public async Task<UserProfile> ReadAsync(string userId)
        {
            var _profile = new UserProfile();
            var _UserRole=await _context.UserRoles.FirstOrDefaultAsync(x=>x.UserId==userId);//får ut role id från userrole
            var _roleid = _UserRole.RoleId;
            var _role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == _roleid);//får ut role
            
            var _profilentity = await _context.Profiles.Include(x=>x.User).FirstOrDefaultAsync(x=>x.UserId==userId);
            if (_profilentity != null)
            {

                _profile.FirstName = _profilentity.FirstName;
                _profile.LastName = _profilentity.LastName;
                _profile.Email = _profilentity.User.Email;
                _profile.StreetName = _profilentity.StreetName;
                _profile.PostalCode = _profilentity.PostalCode;
                _profile.City = _profilentity.City;
                _profile.Country = _profilentity.Country;
                _profile.UserRole = _role.Name;

            }
            return _profile;
        }

        //public async Task<string> Edit(string id,UpdateUserProfile profile)
        //{

        //    try
        //    {
        //        var userProfileEntity = new ProfileEntity
        //        {
        //            UserId = id,
        //            FirstName = profile.FirstName,
        //            LastName = profile.LastName,
        //            //Email = profile.Email
        //        };

        //        _context.Profiles.Update(userProfileEntity);
        //        await _context.SaveChangesAsync();

        //        return id;
        //    }
        //    catch
        //    {
        //        return id;
        //    }
        //}
    }


    public class ProfileResult
    {
        public bool Succeeded { get; set; } = false;
    }
}

