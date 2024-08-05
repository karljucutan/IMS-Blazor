using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.Accounts.EFCoreSqlserver
{
    public class AccountsDbContext(DbContextOptions<AccountsDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
    }
}
