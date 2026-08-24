using Microsoft.AspNetCore.Identity;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Infrastructure.Persistence.Seed;

public class IdentitySeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public IdentitySeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[]
        {
            Roles.Admin,
            Roles.Customer
        };

        foreach (var role in roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
                continue;

            var result = await _roleManager.CreateAsync(
                new IdentityRole<Guid>(role));

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Failed to create role '{role}': {errors}");
            }
        }
    }

    private async Task SeedAdminAsync()
    {
        const string email = "admin@admin.com";
        const string password = "Admin@Store@123";

        var admin = await _userManager.FindByEmailAsync(email);

        if (admin is not null)
            return;

        admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User"
        };

        var result = await _userManager.CreateAsync(admin, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(e => e.Description));

            throw new InvalidOperationException(
                $"Failed to create admin '{email}': {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(
            admin,
            Roles.Admin);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(
                ", ",
                roleResult.Errors.Select(e => e.Description));

            throw new InvalidOperationException(
                $"Failed to assign Admin role to '{email}': {errors}");
        }
    }
}