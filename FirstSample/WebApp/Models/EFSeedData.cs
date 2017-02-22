using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class EFSeedData
    {
        private EFContext _context;

        public EFSeedData(EFContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
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
