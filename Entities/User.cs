using Microsoft.AspNetCore.Identity;

namespace E_Voting_System.Entities;

public class User : IdentityUser
{
    public int Vote { get; set; }
}
