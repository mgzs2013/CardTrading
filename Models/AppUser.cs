using Microsoft.AspNetCore.Identity;

namespace CardTrading.Models{

    public class AppUser: IdentityUser {
        // public string? LocationName { get; set;}

        public IEnumerable<Card> Cards { get; set; }
    }
}