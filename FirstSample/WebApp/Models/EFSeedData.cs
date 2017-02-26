using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class EFSeedData
    {
        private EFContext _context;
        private UserManager<AppUser> _userManager;

        public EFSeedData(EFContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {
            if(await _userManager.FindByEmailAsync("marek@headchannel.co.uk") == null)
            {
                var user = new AppUser()
                {
                    UserName = "Marek",
                    Email = "marek@headchannel.co.uk"
                };

                await _userManager.CreateAsync(user, "1qazXSW@");
            }

            if(!_context.Contacts.Any())
            {
                var contact_1 = new Contact()
                {
                    Email = "sdfsd@sdfsd.pl",
                    Message = "sdfsdfs",
                    Name = "sdfds"
                };
                _context.Contacts.Add(contact_1);

                await _context.SaveChangesAsync();
            }
        }
    }
}
